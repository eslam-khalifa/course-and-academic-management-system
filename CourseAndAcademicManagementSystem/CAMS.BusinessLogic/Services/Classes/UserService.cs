using CAMS.BusinessLogic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMS.BusinessLogic.Services.Classes
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Business Rule: check if first and last name are not empty or whitespace
            if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName))
                throw new Exception("First and last name are required");

            // Business Rule: check if first and last name are between 3 and 50 characters
            if (user.FirstName.Length < 3 || user.FirstName.Length > 50 ||
                user.LastName.Length < 3 || user.LastName.Length > 50)
                throw new Exception("Name must be 3-50 characters");

            // Business Rule: check if email is valid
            if (string.IsNullOrWhiteSpace(user.Email))
                throw new Exception("Email is required");

            // Business Rule: check if email is unique
            if (!await _userRepository.IsEmailUniqueAsync(user.Email))
                throw new Exception("Email already exists");

            // Business Rule: check if role is not empty or whitespace
            if (string.IsNullOrWhiteSpace(user.Role))
                throw new Exception("Role is required");

            return await _userRepository.AddAsync(user);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var existing = await _userRepository.GetByIdAsync(user.UserId);
            if (existing == null) throw new Exception("User not found");

            // Business Rule: check if the new email is not the same as the old one and unique
            if (!string.Equals(existing.Email, user.Email, StringComparison.OrdinalIgnoreCase)
                && !await _userRepository.IsEmailUniqueAsync(user.Email))
                throw new Exception("Email already exists");

            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsDeleted = true;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public Task<User?> GetUserByIdAsync(int userId)
            => _userRepository.GetByIdAsync(userId);

        public Task<IEnumerable<User>> GetUsersAsync(string? role = null, string? search = null, int pageNumber = 1, int pageSize = 10)
            => _userRepository.GetPagedAsync(role, search, pageNumber, pageSize);
    }
}
