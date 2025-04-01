using MilkTeaRepository.Models;

namespace MilkteaServices.UserServices
{
    public interface IUserService
    {
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<User> GetUserByIdAsync(int id);
		Task<User> GetUserByUsernameAsync(string username);
		Task<User> AuthenticateAsync(string username, string password);
		Task<User> RegisterUserAsync(User user, string password);
		Task UpdateUserAsync(User user);
		Task UpdateUserProfileAsync(int id, string fullName, string email, string phone);
		Task ChangePasswordAsync(int id, string currentPassword, string newPassword);
		Task ToggleUserStatusAsync(int id);
		Task DeleteUserAsync(int id);
		Task<bool> IsUsernameUniqueAsync(string username);
		Task<bool> IsEmailUniqueAsync(string email);
	}
}
