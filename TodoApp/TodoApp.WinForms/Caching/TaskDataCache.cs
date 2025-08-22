#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TodoApp.Core.Models;

namespace TodoApp.WinForms.Caching;

/// <summary>
/// A cache to support the DataGridView's Virtual Mode.
/// It retrieves data from a data store in pages (blocks) and caches it for fast access.
/// </summary>
public class TaskDataCache
{
    // The size of the page (block of data) to retrieve from the data store.
    private readonly int _pageSize;
    // The main cache storage. Key is the row index, Value is the TodoItem.
    private readonly Dictionary<int, TodoItem> _cache = new();

    // A delegate that defines the contract for the data fetching logic.
    // The cache will call this delegate whenever it needs a new page of data.
    public delegate Task<List<TodoItem>> DataPageProvider(int pageNumber, int pageSize);
    private readonly DataPageProvider _dataProvider;

    public TaskDataCache(DataPageProvider dataProvider, int pageSize = 100)
    {
        _dataProvider = dataProvider;
        _pageSize = pageSize;
    }

    /// <summary>
    /// Retrieves a TodoItem for a specific row index.
    /// If the item is not in the cache, it fetches the corresponding page from the data store.
    /// </summary>
    /// <param name="rowIndex">The zero-based index of the row to retrieve.</param>
    /// <returns>The TodoItem for the specified row, or null if it cannot be retrieved.</returns>
    public async Task<TodoItem?> GetItemAsync(int rowIndex)
    {
        // Check if the item is already in the cache.
        if (_cache.TryGetValue(rowIndex, out var item))
        {
            return item;
        }

        // If not in cache, calculate which page this row belongs to.
        int pageNumber = (rowIndex / _pageSize) + 1;

        // Fetch the required page of data from the data provider.
        var pageData = await _dataProvider.Invoke(pageNumber, _pageSize);
        if (pageData == null || !pageData.Any())
        {
            return null; // No data returned from the provider.
        }

        // --- Populate the cache with the newly fetched page data ---
        // Calculate the starting index for this page.
        int startIndexOfPage = (pageNumber - 1) * _pageSize;
        for (int i = 0; i < pageData.Count; i++)
        {
            // The key in the cache is the absolute row index.
            _cache[startIndexOfPage + i] = pageData[i];
        }

        // Now that the cache is populated, try to get the item again.
        // This should succeed if the data exists.
        _cache.TryGetValue(rowIndex, out item);
        return item;
    }

    /// <summary>
    /// Removes a specific item from the cache, forcing a reload from the data provider on next access.
    /// </summary>
    /// <param name="rowIndex">The index of the item to invalidate.</param>
    public void InvalidateItem(int rowIndex)
    {
        _cache.Remove(rowIndex);
    }

    /// <summary>
    /// Synchronously tries to retrieve an item from the cache.
    /// Does NOT trigger a fetch from the data store if the item is not present.
    /// This is a lightweight method suitable for use in synchronous UI events like RowPrePaint.
    /// </summary>
    /// <param name="rowIndex">The index of the item to retrieve.</param>
    /// <param name="item">The retrieved item, or null if not found in the cache.</param>
    /// <returns>True if the item was found in the cache; otherwise, false.</returns>
    public bool TryGetItem(int rowIndex, [MaybeNullWhen(false)] out TodoItem item)
    {
        return _cache.TryGetValue(rowIndex, out item);
    }

    /// <summary>
    /// Updates or adds an item at a specific index in the cache.
    /// This is useful for synchronizing the cache with data returned from the server after an update.
    /// </summary>
    /// <param name="rowIndex">The index of the item to update.</param>
    /// <param name="item">The new item to place in the cache.</param>
    public void UpdateItem(int rowIndex, TodoItem item)
    {
        // The dictionary indexer will either update the existing entry or add a new one.
        _cache[rowIndex] = item;
    }

    /// <summary>
    /// Clears the entire cache. This should be called whenever the sorting or filtering changes.
    /// </summary>
    public void Clear()
    {
        _cache.Clear();
    }
}