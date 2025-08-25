#nullable enable
using System.Diagnostics.CodeAnalysis;
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
    private readonly HashSet<int> _pagesBeingFetched = new();

    public TaskDataCache(DataPageProvider dataProvider, int cachePageSize = 20)
    {
        _dataProvider = dataProvider;
        _pageSize = cachePageSize;
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
    /// </summary>
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
    /// Ensures that the data page containing the specified row index is loaded into the cache.
    /// This is the primary method for asynchronous prefetching.
    /// </summary>
    /// <param name="rowIndex">The zero-based index of the row to ensure is loaded.</param>
    /// <returns>The TodoItem for the specified row, or null if it cannot be retrieved.</returns>
    public async Task<TodoItem?> EnsureItemLoadedAsync(int rowIndex)
    {
        // If the item is already in the cache, we're done. Return it immediately.
        if (_cache.TryGetValue(rowIndex, out var item)) return item;

        // Calculate which page this row belongs to.
        int pageNumber = (rowIndex / _pageSize) + 1;

        // --- Prevent redundant fetches for the same page ---
        // If this page is already being fetched by another background task, do nothing and exit.
        lock (_pagesBeingFetched)
        {
            if (_pagesBeingFetched.Contains(pageNumber))
            {
                return null; // Another thread is already handling this page.
            }
            _pagesBeingFetched.Add(pageNumber);
        }

        try
        {
            // Fetch the required page of data from the data provider.
            var pageData = await _dataProvider.Invoke(pageNumber, _pageSize);
            if (pageData == null || !pageData.Any())
            {
                return null; // No data returned from the provider for this page.
            }

            // --- Populate the cache with the newly fetched page data ---
            int startIndexOfPage = (pageNumber - 1) * _pageSize;
            for (int i = 0; i < pageData.Count; i++)
            {
                // The key in the cache is the absolute row index.
                _cache[startIndexOfPage + i] = pageData[i];
            }

            // Now that the cache is populated, try to get the item again.
            _cache.TryGetValue(rowIndex, out item);
            return item;
        }
        catch (Exception ex)
        {
            // Log the error if data fetching fails.
            System.Diagnostics.Debug.WriteLine($"[錯誤] 從資料提供者獲取第 {pageNumber} 頁時失敗: {ex.Message}");
            return null;
        }
        finally
        {
            // --- CRITICAL: Always remove the page from the "fetching" list ---
            // This ensures that if the fetch fails, we can try again later.
            lock (_pagesBeingFetched)
            {
                _pagesBeingFetched.Remove(pageNumber);
            }
        }
    }

    /// <summary>
    /// Clears the entire cache. This should be called whenever the sorting or filtering changes.
    /// </summary>
    public void Clear()
    {
        _cache.Clear();
    }
}