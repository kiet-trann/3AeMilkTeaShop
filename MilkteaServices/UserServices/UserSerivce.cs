using MilkTeaRepository.Models;
using MilkTeaRepository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkteaServices.UserServices
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;

		public UserService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			return await _unitOfWork.GetRepository<User>().GetPaginateAsync();
		}

		public async Task<User> GetUserByIdAsync(int id)
		{
			return await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
		}

		public async Task<User> GetUserByUsernameAsync(string username)
		{
			return await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Username == username);
		}

		public async Task<User> AuthenticateAsync(string username, string password)
		{
			var user = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Username == username);
			if (user == null || user.IsActive == false || user.PasswordHash != password)
				return null;

			return user;
		}

		public async Task<User> RegisterUserAsync(User user, string password)
		{
			var existingUser = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Username == user.Username);
			if (existingUser != null)
				throw new InvalidOperationException("Username is already taken");

			user.PasswordHash = password;
			user.IsActive = true;

			await _unitOfWork.GetRepository<User>().AddAsync(user);
			await _unitOfWork.SaveChangesAsync();

			return user;
		}

		public async Task UpdateUserAsync(User user)
		{
			var existingUser = await _unitOfWork.GetRepository<User>().GetByIdAsync(user.UserId);
			if (existingUser == null)
				throw new KeyNotFoundException($"User with ID {user.UserId} not found");

			existingUser.FullName = user.FullName;
			existingUser.PhoneNumber = user.PhoneNumber;
			existingUser.Role = user.Role;

			await _unitOfWork.GetRepository<User>().UpdateAsync(existingUser);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task UpdateUserProfileAsync(int id, string fullName, string email, string phone)
		{
			var existingUser = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
			if (existingUser == null)
				throw new KeyNotFoundException($"User with ID {id} not found");

			existingUser.FullName = fullName;
			existingUser.PhoneNumber = phone;

			await _unitOfWork.GetRepository<User>().UpdateAsync(existingUser);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task ChangePasswordAsync(int id, string currentPassword, string newPassword)
		{
			var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
			if (user == null)
				throw new KeyNotFoundException($"User with ID {id} not found");

			if (user.PasswordHash != currentPassword)
				throw new InvalidOperationException("Current password is incorrect");

			user.PasswordHash = newPassword;

			await _unitOfWork.GetRepository<User>().UpdateAsync(user);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task ToggleUserStatusAsync(int id)
		{
			var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
			if (user == null)
				throw new KeyNotFoundException($"User with ID {id} not found");

			user.IsActive = !user.IsActive;

			await _unitOfWork.GetRepository<User>().UpdateAsync(user);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task DeleteUserAsync(int id)
		{
			var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
			if (user == null)
				throw new KeyNotFoundException($"User with ID {id} not found");

			await _unitOfWork.GetRepository<User>().RemoveAsync(user);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<bool> IsUsernameUniqueAsync(string username)
		{
			var user = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Username == username);
			return user == null;
		}

		public async Task<bool> IsEmailUniqueAsync(string email)
		{
			var user = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Email == email);
			return user == null;
		}
	}
}
