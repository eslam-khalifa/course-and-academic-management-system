using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
        Task<User?> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetUsersAsync(string? role = null, string? search = null, int pageNumber = 1, int pageSize = 10);
    }
}
