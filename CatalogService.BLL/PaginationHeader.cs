using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.BLL
{
    public class PaginationHeader
    {
        public PaginationHeader(
            int currentPageNumber,
            int pageSize,
            int pageCount,
            int itemCount)
        {
            CurrentPageNumber = currentPageNumber;
            PageSize = pageSize;
            PageCount = pageCount;
            ItemCount = itemCount;
        }

        public int CurrentPageNumber { get; }
        public int ItemCount { get; }
        public int PageSize { get; }
        public int PageCount { get; }
    }
}
