using CAMS.BusinessLogic.ViewModels.CourseViewModels;
using CAMS.BusinessLogic.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.ViewModels.GradeViewModels
{
    public class GradeListViewModel
    {
        public PagedResultViewModel<GradeViewModel> PagedGrades { get; set; } = new PagedResultViewModel<GradeViewModel>();

        public string? SearchTerm { get; set; }
    }
}
