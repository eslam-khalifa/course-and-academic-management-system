using CAMS.BusinessLogic.Services.Interfaces;
using CAMS.BusinessLogic.ViewModels.CourseViewModels;
using CAMS.BusinessLogic.ViewModels.Shared;
using CAMS.DataAccess.Entities;
using DataAccessLayer.Entities;
using DataAccessLayer.IUnitOfWorkAndImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Classes
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOFWork _unitOfWork;

        public CourseService(IUnitOFWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CourseViewModel> CreateCourseAsync(CreatedCourseViewModel courseVm)
        {
            var courseRepo = _unitOfWork.Repository<Course, int>();

            var userRepo = _unitOfWork.Repository<User, int>();

            // Business rule: Name must not contain numbers
            if (courseVm.Name.Any(char.IsDigit))
                throw new Exception("Course name cannot contain numbers");

            // Business rule: Name must be unique
            var existsByName = await courseRepo.ExistsAsync(c => c.Name == courseVm.Name);
            if (existsByName)
                throw new Exception("Course name already exists");

            // Validate Instructor exists
            if (courseVm.InstructorId.HasValue)
            {
                var instructorExists = await userRepo.ExistsAsync(u => u.Id == courseVm.InstructorId.Value);
                if (!instructorExists)
                    throw new Exception("Instructor does not exist");
            }

            // Map VM -> Entity
            var course = new Course
            {
                Name = courseVm.Name,
                Code = courseVm.Code,
                Category = courseVm.Category,
                Description = courseVm.Description,
                Credits = courseVm.Credits,
                DurationHours = courseVm.DurationHours,
                MaxTrainees = courseVm.MaxTrainees,
                IsActive = courseVm.IsActive,
                InstructorId = courseVm.InstructorId,
                ThumbnailUrl = courseVm.ThumbnailUrl
            };

            await courseRepo.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            // Map Entity -> VM
            return new CourseViewModel
            {
                CourseId = course.Id,
                Name = course.Name,
                Code = course.Code,
                Category = course.Category,
                IsActive = course.IsActive,
                InstructorId = course.InstructorId,
                InstructorName = course.Instructor?.Name
            };
        }

        public async Task<CourseViewModel> UpdateCourseAsync(UpdatedCourseViewModel courseVm)
        {
            var courseRepo = _unitOfWork.Repository<Course, int>();
            var userRepo = _unitOfWork.Repository<User, int>();
            var sessionRepo = _unitOfWork.Repository<Session, int>();

            var existing = await courseRepo.GetByIdAsync(courseVm.CourseId);
            if (existing == null) throw new Exception("Course not found");

            // Business rule: Name must not contain numbers
            if (courseVm.Name.Any(char.IsDigit))
                throw new Exception("Course name cannot contain numbers");

            // Business rule: Name must be unique
            var existsByName = await courseRepo.ExistsAsync(c => c.Name == courseVm.Name && c.Id != courseVm.CourseId);
            if (existsByName)
                throw new Exception("Course name already exists");

            // Business rule: Cannot deactivate course with scheduled sessions
            if (!courseVm.IsActive)
            {
                var hasSessions = await sessionRepo.ExistsAsync(s => s.CourseId == courseVm.CourseId );
                if (hasSessions)
                    throw new Exception("Cannot inactivate course with scheduled sessions");
            }

            // Validate Instructor assignment
            if (courseVm.InstructorId.HasValue)
            {
                var instructorExists = await userRepo.ExistsAsync(u => u.Id == courseVm.InstructorId.Value);
                if (!instructorExists)
                    throw new Exception("Instructor does not exist");
            }

            // Map VM -> Entity update
            existing.Name = courseVm.Name;
            existing.Code = courseVm.Code;
            existing.Category = courseVm.Category;
            existing.Description = courseVm.Description;
            existing.Credits = courseVm.Credits;
            existing.DurationHours = courseVm.DurationHours;
            existing.MaxTrainees = courseVm.MaxTrainees;
            existing.IsActive = courseVm.IsActive;
            existing.InstructorId = courseVm.InstructorId;
            existing.ThumbnailUrl = courseVm.ThumbnailUrl;

            await courseRepo.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();

            // Map back to VM
            return new CourseViewModel
            {
                CourseId = existing.Id,
                Name = existing.Name,
                Code = existing.Code,
                Category = existing.Category,
                IsActive = existing.IsActive,
                InstructorId = existing.InstructorId,
                InstructorName = existing.Instructor?.Name
            };
        }

        public async Task<OperationResultViewModel> DeleteCourseAsync(int courseId)
        {
            var courseRepo = _unitOfWork.Repository<Course, int>();
            var course = await courseRepo.GetByIdAsync(courseId);
            if (course == null)
                return OperationResultViewModel.Fail("Course not found");

            // Soft delete
            course.IsDeleted = true;
            await courseRepo.UpdateAsync(course);
            await _unitOfWork.SaveChangesAsync();

            return OperationResultViewModel.Ok("Course deleted successfully");
        }

        public async Task<CourseViewModel?> GetCourseByIdAsync(int courseId)
        {
            var courseRepo = _unitOfWork.Repository<Course, int>();
            var course = await courseRepo.GetByIdAsync(courseId);
            if (course == null) return null;

            return new CourseViewModel
            {
                CourseId = course.Id,
                Name = course.Name,
                Code = course.Code,
                Category = course.Category,
                IsActive = course.IsActive,
                InstructorId = course.InstructorId,
                InstructorName = course.Instructor?.Name
            };
        }

        public async Task<PagedResultViewModel<CourseViewModel>> GetCoursesAsync(string? search = null, int pageNumber = 1, int pageSize = 10)
        {
            var spec=new CourseSpecification(search, pageNumber, pageSize);
            var courseRepo = await _unitOfWork.Repository<Course, int>().GetAllAsync(spec);
          
            return new PagedResultViewModel<CourseViewModel>
            {
                Items = courseRepo.Select(c => new CourseViewModel
                {
                    CourseId = c.Id,
                    Name = c.Name,
                    Code = c.Code,
                    Category = c.Category,
                    IsActive = c.IsActive,
                    InstructorId = c.InstructorId,
                    InstructorName = c.Instructor?.Name,
                }),
                PageNumber = pageNumber,
                PageSize = pageSize,
               
            };
        }
    }
}
