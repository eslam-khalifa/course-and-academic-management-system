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
        Task<Grade> AddGradeAsync(Grade grade);
        Task<Grade> UpdateGradeAsync(Grade grade);
        Task<bool> DeleteGradeAsync(int gradeId); // soft delete
        Task<Grade?> GetGradeByIdAsync(int gradeId);
        Task<IEnumerable<Grade>> GetGradesAsync(int? sessionId = null, int? traineeId = null,
            int pageNumber = 1, int pageSize = 10);
    }
}
