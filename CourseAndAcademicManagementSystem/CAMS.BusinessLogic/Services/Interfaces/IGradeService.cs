using CAMS.BusinessLogic.ViewModels.GradeViewModels;
using CAMS.BusinessLogic.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Interfaces
{
    public interface IGradeService
    {
        Task<GradeViewModel> CreatedGradeAsync(CreatedGradeViewModel gradeVm);
        Task<GradeViewModel> UpdateGradeAsync(UpdatedGradeViewModel grade);
        Task<OperationResultViewModel> DeleteGradeAsync(int gradeId);
        Task<GradeDetailsViewModel?> GetGradeByIdAsync(int gradeId);
        public Task<PagedResultViewModel<GradeViewModel>> GetGradesAsync(string TraineeName,
            int? sessionId = null, int? traineeId = null,
            int pageNumber = 1, int pageSize = 10);
    }
}
