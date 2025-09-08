using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Course> CreateCourseAsync(Course course);
        Task<Course> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(int courseId);
        Task<Course?> GetCourseByIdAsync(int courseId);
        Task<IEnumerable<Course>> GetCoursesAsync(string? search = null, int pageNumber = 1, int pageSize = 10);
    }
}
