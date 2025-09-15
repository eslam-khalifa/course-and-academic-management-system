using CAMS.BusinessLogic.Services.Interfaces;
using CAMS.BusinessLogic.ViewModels.GradeViewModels;
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
    public class GradeService : IGradeService
    {
        private readonly IUnitOFWork _unitOfWork;

        public GradeService(IUnitOFWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GradeViewModel> CreatedGradeAsync(CreatedGradeViewModel gradeVm)
        {
            var gradeRepo = _unitOfWork.Repository<Grade, int>();
            var sessionRepo = _unitOfWork.Repository<Session, int>();
            var userRepo = _unitOfWork.Repository<User, int>();

            // Validate session exists
            var session = await sessionRepo.GetByIdAsync(gradeVm.SessionId);
            if (session == null) throw new Exception("Session does not exist");

            // Cannot add grades to cancelled sessions
            if (session.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Cannot add grade to cancelled session");

            // Validate trainee exists
            var trainee = await userRepo.GetByIdAsync(gradeVm.TraineeId);
            if (trainee is null) throw new Exception("Trainee does not exist");
            if (!trainee.Role.Equals("Trainee", StringComparison.OrdinalIgnoreCase))
                throw new Exception("User is not a trainee");

            if (gradeVm.Value < 0 || gradeVm.Value > 100)
                throw new Exception("Grade value must be between 0 and 100");

            if (gradeVm.IsFinal == true)
            {
                var existingFinal = await gradeRepo.ExistsAsync(
                    g => g.SessionId == gradeVm.SessionId && g.TraineeId == gradeVm.TraineeId && g.IsFinal == gradeVm.IsFinal
                );
                if (existingFinal)
                    throw new Exception("Final grade already exists for this trainee in this session");
            }

            var grade = new Grade
            {
                SessionId = gradeVm.SessionId,
                TraineeId = gradeVm.TraineeId,
                Value = gradeVm.Value,
                Weight = gradeVm.Weight,
                AttemptNumber = gradeVm.AttemptNumber,
                IsFinal = gradeVm.IsFinal,
                Comments = gradeVm.Comments,
                GradedAt = DateTime.UtcNow
            };

            await gradeRepo.AddAsync(grade);
            await _unitOfWork.SaveChangesAsync();

            
            return new GradeViewModel
            {
                GradeId = grade.Id,
                SessionId = grade.SessionId,
                SessionName = session.Title,
                TraineeId = grade.TraineeId,
                TraineeName = trainee.Name,
                Value = grade.Value,
                Weight = grade.Weight,
                IsFinal = grade.IsFinal,
                GradedAt = grade.GradedAt
            };
        }

        public async Task<GradeViewModel> UpdateGradeAsync(UpdatedGradeViewModel gradeVm)
        {
            var gradeRepo = _unitOfWork.Repository<Grade, int>();
            var sessionRepo = _unitOfWork.Repository<Session, int>();
            var userRepo = _unitOfWork.Repository<User, int>();

            var existing = await gradeRepo.GetByIdAsync(gradeVm.GradeId);
            if (existing == null) throw new Exception("Grade not found");

            var session = await sessionRepo.GetByIdAsync(gradeVm.SessionId);
            if (session == null) throw new Exception("Session does not exist");
            if (session.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Cannot update grade for cancelled session");

            var trainee = await userRepo.GetByIdAsync(gradeVm.TraineeId);
            if (trainee is null) throw new Exception("Trainee does not exist");
            if (!trainee.Role.Equals("Trainee", StringComparison.OrdinalIgnoreCase))
                throw new Exception("User is not a trainee");

            if (gradeVm.Value < 0 || gradeVm.Value > 100)
                throw new Exception("Grade value must be between 0 and 100");

            if (gradeVm.IsFinal == true)
            {
                var finalExists = await gradeRepo.ExistsAsync(
                    g => g.Id == gradeVm.GradeId &&
                         g.TraineeId == gradeVm.TraineeId &&
                         g.IsFinal == gradeVm.IsFinal &&
                         g.Id != gradeVm.GradeId
                );
                if (finalExists)
                    throw new Exception("Final grade already exists for this trainee in this session");
            }

            // Map VM -> Entity
            existing.Value = gradeVm.Value;
            existing.Weight = gradeVm.Weight;
            existing.AttemptNumber = gradeVm.AttemptNumber;
            existing.IsFinal = gradeVm.IsFinal;
            existing.Comments = gradeVm.Comments;
            existing.UpdatedAt = DateTime.UtcNow;

            await gradeRepo.UpdateAsync(existing);
            await _unitOfWork.SaveChangesAsync();

            return new GradeViewModel
            {
                GradeId = existing.Id,
                SessionId = existing.SessionId,
                SessionName = session.Title,
                TraineeId = existing.TraineeId,
                TraineeName = trainee.Name,
                Value = existing.Value,
                Weight = existing.Weight,
                IsFinal = existing.IsFinal,
                GradedAt = existing.GradedAt
            };
        }

        public async Task<OperationResultViewModel> DeleteGradeAsync(int gradeId)
        {
            var gradeRepo = _unitOfWork.Repository<Grade, int>();
            var grade = await gradeRepo.GetByIdAsync(gradeId);
            if (grade == null)
                return OperationResultViewModel.Fail("Grade not found");

            grade.IsDeleted = true;
            await gradeRepo.UpdateAsync(grade);
            await _unitOfWork.SaveChangesAsync();

            return OperationResultViewModel.Ok("Grade deleted successfully");
        }

        public async Task<GradeDetailsViewModel?> GetGradeByIdAsync(int gradeId)
        {
            var gradeRepo = _unitOfWork.Repository<Grade, int>();
            var grade = await gradeRepo.GetByIdAsync(gradeId);
            if (grade == null) return null;

            return new GradeDetailsViewModel
            {
                GradeId = grade.Id,
                SessionId = grade.SessionId,
                TraineeId = grade.TraineeId,
                Value = grade.Value,
                Weight = grade.Weight,
                AttemptNumber = grade.AttemptNumber,
                IsFinal = grade.IsFinal,
                GradedAt = grade.GradedAt,
                Comments = grade.Comments,
                CreatedAt = grade.CreatedAt,
                CreatedBy = grade.CreatedBy,
                UpdatedAt = grade.UpdatedAt,
                UpdatedBy = grade.UpdatedBy
            };
        }

        public async Task<PagedResultViewModel<GradeViewModel>> GetGradesAsync(string TraineeName,
            int? sessionId , int? traineeId ,
            int pageNumber = 1, int pageSize = 10)
        {
            var spec = new GradeSpecification(TraineeName, sessionId, traineeId, pageNumber, pageSize);
            var gradeRepo = await _unitOfWork.Repository<Grade, int>().GetAllAsync(spec);
            
           

            return new PagedResultViewModel<GradeViewModel>
            {
                Items = gradeRepo.Select(g => new GradeViewModel
                {
                    GradeId = g.Id,
                    SessionId = g.SessionId,
                    SessionName = g.Session?.Title ?? string.Empty,
                    TraineeId = g.TraineeId,
                    TraineeName = g.Trainee?.Name ?? string.Empty,
                    Value = g.Value,
                    Weight = g.Weight,
                    IsFinal = g.IsFinal,
                    GradedAt = g.GradedAt
                }),
                PageNumber = pageNumber,
                PageSize = pageSize,
               
            };
        }
    }
}
