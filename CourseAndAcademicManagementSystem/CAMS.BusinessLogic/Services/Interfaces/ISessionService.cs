using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace CAMS.BusinessLogic.Services.Interfaces
{
    public interface ISessionService
    {
        Task<Session> CreateSessionAsync(Session session);
        Task<Session> UpdateSessionAsync(Session session);
        Task<bool> DeleteSessionAsync(int sessionId); // soft delete
        Task<Session?> GetSessionByIdAsync(int sessionId);
        Task<IEnumerable<Session>> GetSessionsAsync(int? courseId = null, string? search = null,
            int pageNumber = 1, int pageSize = 10);
    }
}
