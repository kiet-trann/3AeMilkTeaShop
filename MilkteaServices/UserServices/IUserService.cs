using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkTeaWeb.ViewModels;

namespace MilkTea.Services.UserServices
{
    public interface IUserService
    {
		Task<PaginatingResult<User>> GetPaginatedUsersAsync(int pageIndex, int pageSize, string? filter = null);
		Task<UserViewModel> GetUserByIdAsync(int id);
		Task<User> GetUserByUsernameAsync(string username);
		Task<User> AuthenticateAsync(LoginViewModel loginViewModel);
		Task<string> RegisterUserAsync(RegisterViewModel userViewModel);
        Task<string> AddUserAsync(string password, UserViewModel userViewModel);
        Task<string> UpdateUserProfileAsync(UserViewModel userViewModel);
		Task<string> ChangePasswordAsync(PasswordViewModel passwordViewModel);
		Task<string> ToggleUserStatusAsync(int id);
		Task<string> DeleteUserAsync(int id);
		Task<bool> IsUsernameUniqueAsync(string username);
	}
}
