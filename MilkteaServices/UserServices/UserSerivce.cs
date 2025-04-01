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
			return await _unitOfWork.Users.GetAllAsync();
		}

		public async Task<User> GetUserByIdAsync(int id)
		{
			return await _unitOfWork.Users.GetByIdAsync(id);
		}

		public async Task<User> GetUserByUsernameAsync(string username)
		{
			return await _unitOfWork.Users.GetFirstOrDefaultAsync(u => u.Username == username);
		}

		public async Task<User> AuthenticateAsync(string username, string password)
		{
			var user = await _unitOfWork.Users.GetFirstOrDefaultAsync(u => u.Username == username);
			if (user == null || user.IsActive == false || user.Password != password)
				return null;

			return user;
		}

		public async Task<User> RegisterUserAsync(User user, string password)
		{
			var existingUser = await _unitOfWork.Users.GetFirstOrDefaultAsync(u => u.Username == user.Username);
			if (existingUser != null)
				throw new InvalidOperationException("Username is already taken");

			user.Password = password;
			user.IsActive = true;

			await _unitOfWork.Users.AddAsync(user);
			await _unitOfWork.SaveChangesAsync();

			return user;
		}

		public async Task UpdateUserAsync(User user)
		{
			var existingUser = await _unitOfWork.Users.GetByIdAsync(user.UserId);
			if (existingUser == null)
				throw new KeyNotFoundException($"User with ID {user.UserId} not found");

			existingUser.FullName = user.FullName;
			existingUser.Email = user.Email;
			existingUser.Phone = user.Phone;
			existingUser.Role = user.Role;

			await _unitOfWork.Users.UpdateAsync(existingUser);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task UpdateUserProfileAsync(int id, string fullName, string email, string phone)
		{
			var existingUser = await _unitOfWork.Users.GetByIdAsync(id);
			if (existingUser == null)
				throw new KeyNotFoundException($"User with ID {id} not found");

			existingUser.FullName = fullName;
			existingUser.Email = email;
			existingUser.Phone = phone;

			await _unitOfWork.Users.UpdateAsync(existingUser);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task ChangePasswordAsync(int id, string currentPassword, string newPassword)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(id);
			if (user == null)
				throw new KeyNotFoundException($"User with ID {id} not found");

			if (user.Password != currentPassword)
				throw new InvalidOperationException("Current password is incorrect");

			user.Password = newPassword;

			await _unitOfWork.Users.UpdateAsync(user);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task ToggleUserStatusAsync(int id)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(id);
			if (user == null)
				throw new KeyNotFoundException($"User with ID {id} not found");

			user.IsActive = !user.IsActive;

			await _unitOfWork.Users.UpdateAsync(user);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task DeleteUserAsync(int id)
		{
			var user = await _unitOfWork.Users.GetByIdAsync(id);
			if (user == null)
				throw new KeyNotFoundException($"User with ID {id} not found");

			await _unitOfWork.Users.RemoveAsync(user);
			await _unitOfWork.SaveChangesAsync();
		}

		public async Task<bool> IsUsernameUniqueAsync(string username)
		{
			var user = await _unitOfWork.Users.GetFirstOrDefaultAsync(u => u.Username == username);
			return user == null;
		}

		public async Task<bool> IsEmailUniqueAsync(string email)
		{
			var user = await _unitOfWork.Users.GetFirstOrDefaultAsync(u => u.Email == email);
			return user == null;
		}
	}
}
