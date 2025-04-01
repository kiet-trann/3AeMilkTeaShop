using MilkTea.Repository.Model;
using MilkTeaWeb.ViewModels;

namespace MilkTea.Services.UserServices
{
    public interface IUserService
    {
		Task<IEnumerable<User>> GetPaginateUsersAsync(int pageIndex, int pageSize, string? filter = null);
		Task<User> GetUserByIdAsync(int id);
		Task<User> GetUserByUsernameAsync(string username);
		Task<User> AuthenticateAsync(string username, string password);
		Task<string> RegisterUserAsync(RegisterViewModel userViewModel);
		Task<string> UpdateUserProfileAsync(int id, string fullName, string email, string phone);
		Task<string> ChangePasswordAsync(int id, string currentPassword, string newPassword);
		Task<string> ToggleUserStatusAsync(int id);
		Task<string> DeleteUserAsync(int id);
		Task<bool> IsUsernameUniqueAsync(string username);
		Task<bool> IsEmailUniqueAsync(string email);
	}
}
