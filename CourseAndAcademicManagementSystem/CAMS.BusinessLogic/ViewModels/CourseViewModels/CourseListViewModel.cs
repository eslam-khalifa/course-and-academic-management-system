using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.CourseViewModels
{
    public class CourseListViewModel
    {
        public IEnumerable<CourseViewModel> Courses { get; set; } = new List<CourseViewModel>();

        // Search/filter fields
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }

        // Pagination fields
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; }

        // Helper: total pages
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
