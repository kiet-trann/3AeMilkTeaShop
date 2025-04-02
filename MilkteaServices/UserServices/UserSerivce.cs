using AutoMapper;
using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
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

        public async Task<PaginatingResult<User>> GetPaginatedUsersAsync(int pageIndex, int pageSize, string? filter = null)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;

            Expression<Func<User, bool>>? userFilter = null;
            if (!string.IsNullOrWhiteSpace(filter))
            {
                userFilter = u => u.FullName.Contains(filter);
            }

            var totalCount = await _unitOfWork.GetRepository<User>().CountAsync();

            var users = await _unitOfWork.GetRepository<User>()
                .GetPaginateAsync(pageIndex, pageSize, userFilter);

            var pagedResult = new PaginatingResult<User>(users, pageIndex, totalCount, pageSize);
            return pagedResult;
        }

        public async Task<UserViewModel?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserViewModel>(user);
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

        public async Task<string> AddUserAsync(UserViewModel userViewModel)
        {
            if (userViewModel == null)
            {
                return "Thông tin người dùng không hợp lệ";
            }

            // Kiểm tra xem tên người dùng đã tồn tại chưa
            var existingUser = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Username == userViewModel.Username);
            if (existingUser != null)
            {
                return "Tên người dùng đã tồn tại";
            }

            var user = _mapper.Map<User>(userViewModel);
            
            await _unitOfWork.GetRepository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return "Thêm người dùng thành công";
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

        public async Task<string> UpdateUserProfileAsync(int id, string fullName, string phone)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(phone))
                return "Thông tin người dùng không hợp lệ";

            var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
            if (user == null)
                return "Người dùng không tồn tại";

            // Cập nhật thông tin người dùng
            user.FullName = fullName;
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

    }
}
