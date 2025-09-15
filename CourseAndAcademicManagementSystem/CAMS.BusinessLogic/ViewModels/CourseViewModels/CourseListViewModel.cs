using CAMS.BusinessLogic.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.CourseViewModels
{
    public class CourseListViewModel
    {
        public PagedResultViewModel<CourseViewModel> PagedCourses { get; set; } = new PagedResultViewModel<CourseViewModel>();

        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
    }
}
