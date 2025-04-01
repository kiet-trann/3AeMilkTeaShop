using AutoMapper;
using MilkTea.Repository.Model;
using MilkTeaRepository.UnitOfWork;
using MilkTeaWeb.ViewModels;
using System.Linq.Expressions;

namespace MilkTea.Services.UserServices
{
	public class UserService : IUserService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UserService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<IEnumerable<User>> GetPaginateUsersAsync(int pageIndex, int pageSize, string? filter = null)
		{
			if(pageIndex <= 1) pageIndex = 1;
			if(pageSize <= 1) pageSize = 10;

			Expression<Func<User, bool>>? userFilter = null;

			if (!string.IsNullOrWhiteSpace(filter))
			{
				userFilter = u => u.FullName.Contains(filter);
			}
			
			return await _unitOfWork.GetRepository<User>().GetPaginateAsync(
				pageNumber: pageIndex,
				pageSize: pageSize,
				filter: userFilter
			);
		}

		public async Task<User?> GetUserByIdAsync(int id)
		{
			return await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
		}

		public async Task<User?> GetUserByUsernameAsync(string username)
		{
			if (string.IsNullOrWhiteSpace(username))
				return null;

			return await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Username == username);
		}

		public async Task<User?> AuthenticateAsync(string username, string password)
		{
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
				return null;

			// Tìm kiếm người dùng theo username hoặc email
			var user = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Username == username);

			if (user == null || !user.IsActive)
				return null;

			// Kiểm tra mật khẩu
			return user.Password == password ? user : null;
		}

		public async Task<string> RegisterUserAsync(RegisterViewModel userViewModel)
		{
			if (userViewModel == null)
			{
				return "Thông tin người dùng không hợp lệ";
			}

			var existingUser = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Username == userViewModel.Username);
			if (existingUser != null)
			{
				return "Tên người dùng đã tồn tại";
			}

			User user = _mapper.Map<User>(userViewModel);

			// Đặt Role mặc định là "customer"
			user.Role = "Customer";
			user.IsActive = true;

			await _unitOfWork.GetRepository<User>().AddAsync(user);
			await _unitOfWork.SaveChangesAsync();

			return "Đăng ký thành công";
		}

		public async Task<string> ChangePasswordAsync(int id, string currentPassword, string newPassword)
		{
			if (string.IsNullOrWhiteSpace(newPassword))
				return "Mật khẩu mới không được để trống";

			var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
			if (user == null)
				return "Người dùng không tồn tại";

			if (user.Password != currentPassword)
				return "Mật khẩu hiện tại không đúng";

			user.Password = newPassword;

			await _unitOfWork.GetRepository<User>().UpdateAsync(user);
			await _unitOfWork.SaveChangesAsync();
			return "Đổi mật khẩu thành công";
		}

		public async Task<string> ToggleUserStatusAsync(int id)
		{
			var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
			if (user == null)
				return "Người dùng không tồn tại";

			user.IsActive = !user.IsActive;

			await _unitOfWork.GetRepository<User>().UpdateAsync(user);
			await _unitOfWork.SaveChangesAsync();
			return "Cập nhật trạng thái thành công";
		}

		public async Task<string> UpdateUserProfileAsync(int id, string fullName, string email, string phone)
		{
			if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone))
				return "Thông tin người dùng không hợp lệ";

			var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
			if (user == null)
				return "Người dùng không tồn tại";

			// Kiểm tra xem email đã tồn tại hay chưa
			var existingEmailUser = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Email == email && u.UserId != id);
			if (existingEmailUser != null)
				return "Email đã được sử dụng bởi người dùng khác";

			// Cập nhật thông tin người dùng
			user.FullName = fullName;
			user.Email = email;
			user.PhoneNumber = phone;

			await _unitOfWork.GetRepository<User>().UpdateAsync(user);
			await _unitOfWork.SaveChangesAsync();

			return "Cập nhật thông tin người dùng thành công";
		}

		public async Task<string> DeleteUserAsync(int id)
		{
			var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
			if (user == null)
				return "Người dùng không tồn tại";

			await _unitOfWork.GetRepository<User>().RemoveAsync(user);
			await _unitOfWork.SaveChangesAsync();
			return "Xóa người dùng thành công";
		}

		public async Task<bool> IsUsernameUniqueAsync(string username)
		{
			if (string.IsNullOrWhiteSpace(username))
				return false;

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
