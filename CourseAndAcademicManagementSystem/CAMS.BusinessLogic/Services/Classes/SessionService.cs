using CAMS.BusinessLogic.Services.Interfaces;
using CAMS.BusinessLogic.ViewModels.SessionViewModels;
using CAMS.BusinessLogic.ViewModels.Shared;
using CAMS.DataAccess.Entities;
using DataAccessLayer.Entities;
using DataAccessLayer.IUnitOfWorkAndImplementation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOFWork _unitOfWork;

        public SessionService(IUnitOFWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SessionViewModel> CreateSessionAsync(CreatedSessionViewModel vm)
        {
            var sessionRepo = _unitOfWork.Repository<Session, int>();
            var courseRepo = _unitOfWork.Repository<Course, int>();
            var userRepo = _unitOfWork.Repository<User, int>();

            // Business Rule: Validate Course exists
            var course = await courseRepo.GetByIdAsync(vm.CourseId);
            if (course == null) throw new Exception("Course does not exist");

            // Business Rule: Validate Instructor exists
            var instructorExists = await userRepo.ExistsAsync(u => u.Id == vm.InstructorId);
            if (!instructorExists) throw new Exception("Instructor does not exist");

            // Business Rule: Validate dates
            if (vm.EndDate <= vm.StartDate)
                throw new Exception("End date must be after Start date");

            if (vm.Capacity < 0)
                throw new Exception("Capacity cannot be negative");

            var session = new Session
            {
                CourseId = vm.CourseId,
                Title = vm.Title,
                SessionCode = vm.SessionCode,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                EnrollmentStartDate = vm.EnrollmentStartDate,
                EnrollmentEndDate = vm.EnrollmentEndDate,
                Location = vm.Location,
                Capacity = vm.Capacity,
                InstructorId = vm.InstructorId,
                Status = vm.Status,
                Mode = vm.Mode
            };

            await sessionRepo.AddAsync(session);
            await _unitOfWork.SaveChangesAsync();

            return new SessionViewModel
            {
                SessionId = session.Id,
                CourseId = session.CourseId,
                Title = session.Title,
                SessionCode = session.SessionCode,
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                InstructorId = session.InstructorId,
                InstructorName = session.Instructor?.Name,
                Status = session.Status
            };
        }

        public async Task<SessionViewModel> UpdateSessionAsync(UpdatedSessionViewModel vm)
        {
            var sessionRepo = _unitOfWork.Repository<Session, int>();
            var courseRepo = _unitOfWork.Repository<Course, int>();
            var userRepo = _unitOfWork.Repository<User, int>();

            var existing = await sessionRepo.GetByIdAsync(vm.SessionId);
            if (existing == null) throw new Exception("Session not found");

            var course = await courseRepo.GetByIdAsync(vm.CourseId);
            if (course == null) throw new Exception("Course does not exist");

            var instructorExists = await userRepo.ExistsAsync(u => u.Id == vm.InstructorId);
            if (!instructorExists) throw new Exception("Instructor does not exist");

            if (vm.EndDate <= vm.StartDate)
                throw new Exception("End date must be after Start date");

            if (vm.Capacity < 0)
                throw new Exception("Capacity cannot be negative");

            existing.Title = vm.Title;
            existing.SessionCode = vm.SessionCode;
            existing.StartDate = vm.StartDate;
            existing.EndDate = vm.EndDate;
            existing.EnrollmentStartDate = vm.EnrollmentStartDate;
            existing.EnrollmentEndDate = vm.EnrollmentEndDate;
            existing.Location = vm.Location;
            existing.Capacity = vm.Capacity;
            existing.InstructorId = vm.InstructorId;
            existing.Status = vm.Status;
            existing.Mode = vm.Mode;

            await sessionRepo.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();

            return new SessionViewModel
            {
                SessionId = existing.Id,
                CourseId = existing.CourseId,
                Title = existing.Title,
                SessionCode = existing.SessionCode,
                StartDate = existing.StartDate,
                EndDate = existing.EndDate,
                InstructorId = existing.InstructorId,
                InstructorName = existing.Instructor?.Name,
                Status = existing.Status
            };
        }

        public async Task<OperationResultViewModel> DeleteSessionAsync(int sessionId)
        {
            var sessionRepo = _unitOfWork.Repository<Session, int>();
            var session = await sessionRepo.GetByIdAsync(sessionId);
            if (session == null)
                return OperationResultViewModel.Fail("Session not found");

            session.IsDeleted = true;
            await sessionRepo.UpdateAsync(session);
            await _unitOfWork.SaveChangesAsync();

            return OperationResultViewModel.Ok("Session deleted successfully");
        }

        public async Task<SessionDetailsViewModel?> GetSessionByIdAsync(int sessionId)
        {
            var sessionRepo = _unitOfWork.Repository<Session, int>();
            var session = await sessionRepo.GetByIdAsync(sessionId);
            if (session == null) return null;

            return new SessionDetailsViewModel
            {
                SessionId = session.Id,
                CourseId = session.CourseId,
                Course = session.Course!,
                Title = session.Title,
                SessionCode = session.SessionCode,
                StartDate = session.StartDate,
                EndDate = session.EndDate,
                EnrollmentStartDate = session.EnrollmentStartDate,
                EnrollmentEndDate = session.EnrollmentEndDate,
                Location = session.Location,
                Capacity = session.Capacity,
                InstructorId = session.InstructorId,
                Status = session.Status,
                Mode = session.Mode,
                CreatedAt = session.CreatedAt,
                CreatedBy = session.CreatedBy,
                UpdatedAt = session.UpdatedAt,
                UpdatedBy = session.UpdatedBy
            };
        }

        public async Task<PagedResultViewModel<SessionViewModel>> GetSessionsAsync(
            int? courseId ,
            string? search ,
            int pageNumber = 1,
            int pageSize = 5)
        {
           var spec = new SessionSpecification(courseId, search, pageNumber, pageSize);
            var items = await _unitOfWork.Repository<Session, int>().GetAllAsync(spec);

            return new PagedResultViewModel<SessionViewModel>
            {
                Items = items.Select(s => new SessionViewModel
                {
                    SessionId = s.Id,
                    CourseId = s.CourseId,
                    Title = s.Title,
                    SessionCode = s.SessionCode,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    InstructorId = s.InstructorId,
                    InstructorName = s.Instructor?.Name,
                    Status = s.Status
                }),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
        }
    }
}
