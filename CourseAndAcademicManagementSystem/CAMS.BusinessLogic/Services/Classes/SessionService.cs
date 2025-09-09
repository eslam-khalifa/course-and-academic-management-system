using CAMS.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace CAMS.BusinessLogic.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepoistory;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;

        public SessionService(ISessionRepository sessionRepository, ICourseRepository courseRepository, IUserRepository userRepository)
        {
            _sessionRepoistory = sessionRepository;
            _courseRepository = courseRepository;
            _userRepository = userRepository;
        }

        public async Task<Session> CreateSessionAsync(Session session)
        {
            // Business Rule: Validate Course exists
            var course = await _courseRepository.GetByIdAsync(session.CourseId);
            if (course == null) throw new Exception("Course does not exist");

            // Business Rule: Validate Instructor exists if provided
            if (session.InstructorId.HasValue)
            {
                var instructorExists = await _userRepository.ExistsByIdAsync(session.InstructorId.Value);
                if (!instructorExists) throw new Exception("Instructor does not exist");
            }

            // Business Rule: Validate dates
            if (session.StartDate < DateTime.Today)
                throw new Exception("Start date cannot be in the past");
            if (session.EndDate <= session.StartDate)
                throw new Exception("End date must be after Start date");

            // Business Rule: Validate Capacity
            if (session.Capacity < 0)
                throw new Exception("Capacity cannot be negative");

            return await _sessionRepoistory.AddAsync(session);
        }

        public async Task<Session> UpdateSessionAsync(Session session)
        {
            // Business Rule: check if session exists
            var existing = await _sessionRepoistory.GetByIdAsync(session.SessionId);
            if (existing == null) throw new Exception("Session not found");

            // Business Rule: Validate Course exists
            var course = await _courseRepository.GetByIdAsync(session.CourseId);
            if (course == null) throw new Exception("Course does not exist");

            // Business Rule: Validate Instructor exists if provided
            if (session.InstructorId.HasValue)
            {
                var instructorExists = await _userRepository.ExistsByIdAsync(session.InstructorId.Value);
                if (!instructorExists) throw new Exception("Instructor does not exist");
            }

            // Business Rule: Validate dates
            if (session.StartDate < DateTime.Today)
                throw new Exception("Start date cannot be in the past");
            if (session.EndDate <= session.StartDate)
                throw new Exception("End date must be after Start date");

            // Business Rule: Validate Capacity
            if (session.Capacity < 0)
                throw new Exception("Capacity cannot be negative");

            return await _sessionRepoistory.UpdateAsync(session);
        }

        public async Task<bool> DeleteSessionAsync(int sessionId)
        {
            var session = await _sessionRepoistory.GetByIdAsync(sessionId);
            if (session == null) return false;

            session.IsDeleted = true;
            await _sessionRepoistory.UpdateAsync(session);
            return true;
        }

        public Task<Session?> GetSessionByIdAsync(int sessionId)
            => _sessionRepoistory.GetByIdAsync(sessionId);

        public Task<IEnumerable<Session>> GetSessionsAsync(int? courseId = null, string? search = null,
            int pageNumber = 1, int pageSize = 10)
            => _sessionRepoistory.GetPagedAsync(courseId, search, pageNumber, pageSize);
    }
}
