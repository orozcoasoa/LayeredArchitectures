using System.Collections;
using System.Text.Json;

namespace CatalogService.BLL
{
    public class PagedCollection<T> : IPagedCollection<T>
    {
        private readonly List<T> _list = new List<T>();

        public int CurrentPageNumber { get; set; }
        public int ItemCount { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public int LastPageNumber => PageCount;
        public int? NextPageNumber => HasNext ? CurrentPageNumber + 1 : default(int?);
        public int? PreviousPageNumber => HasPrevious ? CurrentPageNumber - 1 : default(int?);
        public bool HasPrevious => CurrentPageNumber > 1;
        public bool HasNext => CurrentPageNumber < PageCount;

        public PagedCollection(IReadOnlyList<T> items, int itemCount, int pageNumber, int pageSize)
        {
            ItemCount = itemCount;
            CurrentPageNumber = pageNumber;
            PageSize = pageSize;
            PageCount = (PageSize > 0 ? (int)Math.Ceiling(ItemCount / (double)PageSize) : 0);
            _list.AddRange(items);
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        public string ToPaginationHeader()
        {
            var paginationHeader = new PaginationHeader(CurrentPageNumber, PageSize, PageCount, ItemCount);
            var jsonHeader = JsonSerializer.Serialize(paginationHeader);
            return jsonHeader;
        }
    }
}
