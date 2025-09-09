using CAMS.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Classes
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;

        public GradeService(IGradeRepository gradeRepository, ISessionRepository sessionRepository, IUserRepository userRepository)
        {
            _gradeRepository = gradeRepository;
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        public async Task<Grade> AddGradeAsync(Grade grade)
        {
            // Business Rule: Validate Session exists
            var session = await _sessionRepository.GetByIdAsync(grade.SessionId);
            if (session == null) throw new Exception("Session does not exist");

            // Business Rule: Cannot add grades to cancelled sessions
            if (session.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Cannot add grade to cancelled session");

            // Business Rule: Validate Trainee exists and role
            var trainee = await _userRepository.GetByIdAsync(grade.TraineeId);
            if (trainee == null) throw new Exception("Trainee does not exist");
            if (!trainee.Role.Equals("Trainee", StringComparison.OrdinalIgnoreCase))
                throw new Exception("User is not a trainee");

            // Business Rule: Validate value
            if (grade.Value < 0 || grade.Value > 100)
                throw new Exception("Grade value must be between 0 and 100");

            // Business Rule: Validate unique IsFinal per session + trainee
            if (grade.IsFinal)
            {
                var finalExists = (await _gradeRepository.GetGradesBySessionAndTraineeAsync(grade.SessionId, grade.TraineeId))
                                  .Any(g => g.IsFinal);
                if (finalExists) throw new Exception("Final grade already exists for this trainee in this session");
            }

            return await _gradeRepository.AddAsync(grade);
        }

        public async Task<Grade> UpdateGradeAsync(Grade grade)
        {
            var existing = await _gradeRepository.GetByIdAsync(grade.GradeId);
            if (existing == null) throw new Exception("Grade not found");

            // Business Rule: Validate Session exists
            var session = await _sessionRepository.GetByIdAsync(grade.SessionId);
            if (session == null) throw new Exception("Session does not exist");

            // Business Rule: Cannot add grades to cancelled sessions
            if (session.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Cannot add grade to cancelled session");

            // Business Rule: Validate Trainee exists and role
            var trainee = await _userRepository.GetByIdAsync(grade.TraineeId);
            if (trainee == null) throw new Exception("Trainee does not exist");
            if (!trainee.Role.Equals("Trainee", StringComparison.OrdinalIgnoreCase))
                throw new Exception("User is not a trainee");

            // Business Rule: Validate value
            if (grade.Value < 0 || grade.Value > 100)
                throw new Exception("Grade value must be between 0 and 100");

            // Business Rule: Validate unique IsFinal per session + trainee
            if (grade.IsFinal)
            {
                var finalExists = (await _gradeRepository.GetGradesBySessionAndTraineeAsync(grade.SessionId, grade.TraineeId))
                                  .Any(g => g.IsFinal);
                if (finalExists) throw new Exception("Final grade already exists for this trainee in this session");
            }

            return await _sessionRepoistory.UpdateAsync(session);
        }

        public async Task<bool> DeleteGradeAsync(int gradeId)
        {
            var grade = await _gradeRepository.GetByIdAsync(gradeId);
            if (grade == null) return false;

            grade.IsDeleted = true;
            await _gradeRepository.UpdateAsync(grade);
            return true;
        }

        public Task<Grade?> GetGradeByIdAsync(int gradeId)
            => _gradeRepository.GetByIdAsync(gradeId);

        public Task<IEnumerable<Grade>> GetGradesAsync(int? sessionId = null, int? traineeId = null,
            int pageNumber = 1, int pageSize = 10)
            => _gradeRepository.GetPagedAsync(sessionId, traineeId, pageNumber, pageSize);
    }
}
