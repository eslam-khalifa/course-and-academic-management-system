using CAMS.BusinessLogic.ViewModels.CourseViewModels;
using CAMS.BusinessLogic.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Interfaces
{
    public interface ICourseService
    {
        Task<CourseViewModel> CreateCourseAsync(CreatedCourseViewModel course);
        Task<CourseViewModel> UpdateCourseAsync(UpdatedCourseViewModel course);
        Task<OperationResultViewModel> DeleteCourseAsync(int courseId);
        Task<CourseViewModel?> GetCourseByIdAsync(int courseId);
        public Task<PagedResultViewModel<CourseViewModel>> GetCoursesAsync(string? search = null, int pageNumber = 1, int pageSize = 10);
    }
}
