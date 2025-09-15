using CAMS.BusinessLogic.ViewModels;
using CAMS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserVM>> GetAllAsync(QueryUser queryUser);
        //Task<int> GetCountAsync(QueryUser queryUser);
        Task<UserVM?> GetByIdAsync(int id);
        Task<UserVM> CreateAsync(UserVM user);
        Task<UserVM?> UpdateAsync(UserVM user);
        Task<UserVM> DeleteAsync(int id);
       // Task<bool> IsEmailUniqueAsync(string email, int excludeId = 0);
    }
}

