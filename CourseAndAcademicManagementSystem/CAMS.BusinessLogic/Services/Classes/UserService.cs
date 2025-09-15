using CAMS.BusinessLogic.Services.Interfaces;
using CAMS.BusinessLogic.ViewModels;
using CAMS.DataAccess.Entities;
using DataAccessLayer.DbContexts;
using DataAccessLayer.IUnitOfWorkAndImplementation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CAMS.BusinessLogic.Services.Classes
{
    public class UserService:IUserService
    {
        private readonly IUnitOFWork _unitOFWork;
        private readonly ILogger<IUserService> _logger;

        public UserService(IUnitOFWork unitOFWork,ILogger<IUserService> logger)
        {
           
            _unitOFWork = unitOFWork;
            _logger = logger;
        }

        public async Task<IEnumerable<UserVM>> GetAllAsync(QueryUser queryUser)
        {
            var  spec= new UserSpecification(queryUser);
            var Data = await _unitOFWork.Repository<User, int>().GetAllAsync(spec);
            return Data.Select(data => new UserVM
            {
                Id = data.Id,
                Name = data.Name,
                Email = data.Email,
                Role = data.Role
            });

        }

        public async Task<UserVM?> GetByIdAsync(int id)
        {
            var data = await _unitOFWork.Repository<User, int>().GetByIdAsync(id);
            return new UserVM
            {
                Id  = data.Id,
                Name = data.Name,
                Email = data.Email,
                Role= data.Role
            };
        }

        public async Task<UserVM> CreateAsync(UserVM userVM)
        {
            var data = await _unitOFWork.Repository<User, int>().GetByIdAsync(userVM.Id);
            if(data != null)
            {
                throw new Exception("User with the same ID already exists");
            }
            try
            {
                var user = new User
                {
                    Name = userVM.Name,
                    Email = userVM.Email,
                    Role = userVM.Role
                };
                await _unitOFWork.Repository<User, int>().AddAsync(user);
                await _unitOFWork.SaveChangesAsync();
                return userVM;


            }
            catch (Exception ex)
            {
                throw new Exception("Error creating user");
            }
             
        }


        public async Task<UserVM?> UpdateAsync(UserVM userVM)
        {
            var data = await _unitOFWork.Repository<User, int>().GetByIdAsync(userVM.Id);
            if (data != null)
            {
                throw new Exception("User with the same ID already exists");
            }
            try
            {
                var user = new User
                {
                    Id = userVM.Id,
                    Name = userVM.Name,
                    Email = userVM.Email,
                    Role = userVM.Role
                };
                await _unitOFWork.Repository<User, int>().UpdateAsync(user);
                await _unitOFWork.SaveChangesAsync();
                return userVM;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Updating user");
            }
        }

        public async Task<UserVM> DeleteAsync(int id)
        {
            var data = await _unitOFWork.Repository<User, int>().GetByIdAsync(id);
            if (data == null)
            {
               throw new Exception("User not found");
            }
            try
            {
                
                await _unitOFWork.Repository<User, int>().DeleteAsync(data);
                await _unitOFWork.SaveChangesAsync();
                return new UserVM
                {
                    Id = data.Id,
                    Name = data.Name,
                    Email = data.Email,
                    Role = data.Role
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error Delete user");
            }
        }

        

        //public async Task<bool> IsEmailUniqueAsync(string email, int excludeId = 0)
        //{
        //    return !await _context.Users.AnyAsync(u => u.Email == email && u.Id != excludeId);
        //}
    }
}
