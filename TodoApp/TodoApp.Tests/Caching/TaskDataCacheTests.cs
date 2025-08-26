#nullable enable
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Core.Models;
using TodoApp.Infrastructure.Caching;

namespace TodoApp.Tests.NUnit;

[TestFixture]
public class TaskDataCacheTests
{
    private List<TodoItem> _fullDataSet = null!;

    [SetUp]
    public void Setup()
    {
        // Create a large, predictable data set for testing.
        _fullDataSet = Enumerable.Range(0, 500)
            .Select(i => new TodoItem { Id = i + 1, Title = $"Task {i + 1}" })
            .ToList();
    }

    [Test]
    public async Task EnsureItemLoadedAsync_FirstRequest_FetchesFirstPage()
    {
        // Arrange
        var pageSize = 100;
        var requestedIndex = 5;
        var timesCalled = 0;

        // Create a fake data provider that increments a counter.
        Task<List<TodoItem>> fakeDataProvider(int pageNumber, int pSize)
        {
            timesCalled++;
            Assert.That(pageNumber, Is.EqualTo(1)); // Expect page 1
            Assert.That(pSize, Is.EqualTo(pageSize));

            var page = _fullDataSet.Skip((pageNumber - 1) * pSize).Take(pSize).ToList();
            return Task.FromResult(page);
        }

        var cache = new TaskDataCache(fakeDataProvider, pageSize);

        // Act
        var item = await cache.EnsureItemLoadedAsync(requestedIndex);

        // Assert
        Assert.That(timesCalled, Is.EqualTo(1), "Data provider should be called exactly once.");
        Assert.That(item, Is.Not.Null);
        Assert.That(item.Id, Is.EqualTo(requestedIndex + 1));
    }

    [Test]
    public async Task EnsureItemLoadedAsync_CacheHit_DoesNotFetchAgain()
    {
        // Arrange
        var pageSize = 100;
        var timesCalled = 0;

        Task<List<TodoItem>> fakeDataProvider(int pageNumber, int pSize)
        {
            timesCalled++;
            var page = _fullDataSet.Skip((pageNumber - 1) * pSize).Take(pSize).ToList();
            return Task.FromResult(page);
        }

        var cache = new TaskDataCache(fakeDataProvider, pageSize);

        // Act
        await cache.EnsureItemLoadedAsync(10); // First call, fetches page 1
        await cache.EnsureItemLoadedAsync(20); // Second call, should be a cache hit

        // Assert
        Assert.That(timesCalled, Is.EqualTo(1), "Data provider should only be called once for the same page.");
    }

    [Test]
    public async Task EnsureItemLoadedAsync_RequestDeduplication_FetchesOnlyOnce()
    {
        // Arrange
        var pageSize = 100;
        var timesCalled = 0;

        Task<List<TodoItem>> fakeDataProvider(int pageNumber, int pSize)
        {
            timesCalled++;
            // Simulate network latency
            return Task.Delay(100).ContinueWith(_ =>
                _fullDataSet.Skip((pageNumber - 1) * pSize).Take(pSize).ToList()
            );
        }

        var cache = new TaskDataCache(fakeDataProvider, pageSize);

        // Act: Fire off multiple requests for the same page concurrently.
        var task1 = cache.EnsureItemLoadedAsync(10);
        var task2 = cache.EnsureItemLoadedAsync(20);
        var task3 = cache.EnsureItemLoadedAsync(30);

        await Task.WhenAll(task1, task2, task3);

        // Assert
        Assert.That(timesCalled, Is.EqualTo(1), "Data provider should only be called once despite concurrent requests for the same page.");
    }
}