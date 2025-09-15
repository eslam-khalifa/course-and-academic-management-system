using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.Shared
{
    public class PagedResultViewModel<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }

        public bool HasPreviousPage => PageNumber > 1; // omar

        public bool HasNextPage => PageNumber < TotalPages; // omar

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
