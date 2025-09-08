using CAMS.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Classes
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;

        public CourseService(ICourseRepository courseRepository, ISessionRepository sessionRepository, IUserRepository userRepository)
        {
            _courseRepository = courseRepository;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            // Business rule: Name must not contain numbers
            if (course.Name.Any(char.IsDigit))
                throw new Exception("Course name cannot contain numbers");

            // Business rule: Name must be unique
            if (await _courseRepository.ExistsByNameAsync(course.Name))
                throw new Exception("Course name already exists");

            // Optional: validate Instructor exists
            if (course.InstructorId.HasValue)
            {
                var exists = await _userRepository.ExistsByIdAsync(course.InstructorId.Value);
                if (!exists)
                    throw new Exception("Instructor does not exist");
            }

            return await _courseRepository.AddAsync(course);
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            var existing = await _courseRepository.GetByIdAsync(course.CourseId);
            if (existing == null) throw new Exception("Course not found");

            // Business rule: Name must not contain numbers
            if (course.Name.Any(char.IsDigit))
                throw new Exception("Course name cannot contain numbers");

            // Business rule: Name must be unique
            if (await _courseRepository.ExistsByNameAsync(course.Name) &&
                !string.Equals(existing.Name, course.Name, StringComparison.OrdinalIgnoreCase))
                throw new Exception("Course name already exists");

            // Business rule: Cannot deactivate course with scheduled sessions
            if (!course.IsActive)
            {
                var scheduledSessions = await _sessionRepository.HasScheduledSessionsAsync(course.CourseId);
                if (scheduledSessions)
                    throw new Exception("Cannot inactivate course with scheduled sessions");
            }

            // Validate Instructor assignment
            if (course.InstructorId.HasValue)
            {
                var exists = await _userRepository.ExistsByIdAsync(course.InstructorId.Value);
                if (!exists)
                    throw new Exception("Instructor does not exist");
            }

            return await _courseRepository.UpdateAsync(course);
        }

        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            if (course == null) return false;

            // Soft delete
            course.IsDeleted = true;
            await _courseRepository.UpdateAsync(course);
            return true;
        }

        public Task<Course?> GetCourseByIdAsync(int courseId)
            => _courseRepository.GetByIdAsync(courseId);

        public Task<IEnumerable<Course>> GetCoursesAsync(string? search = null, int pageNumber = 1, int pageSize = 10)
            => _courseRepository.GetPagedAsync(search, pageNumber, pageSize);
    }
}
