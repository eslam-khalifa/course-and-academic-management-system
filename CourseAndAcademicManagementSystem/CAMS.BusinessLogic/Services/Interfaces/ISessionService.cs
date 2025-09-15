using CAMS.BusinessLogic.ViewModels.SessionViewModels;
using CAMS.BusinessLogic.ViewModels.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Interfaces
{
    public interface ISessionService
    {
        Task<SessionViewModel> CreateSessionAsync(CreatedSessionViewModel vm);
        Task<SessionViewModel> UpdateSessionAsync(UpdatedSessionViewModel vm);
        Task<OperationResultViewModel> DeleteSessionAsync(int sessionId);
        Task<SessionDetailsViewModel?> GetSessionByIdAsync(int sessionId);
        Task<PagedResultViewModel<SessionViewModel>> GetSessionsAsync(
            int? courseId = null,
            string? search = null,
            int pageNumber = 1,
            int pageSize = 10);
        Task<IEnumerable<SelectListItem>> GetCoursesForDropdownAsync();
        Task<IEnumerable<SelectListItem>> GetInstructorsForDropdownAsync();
    }
}