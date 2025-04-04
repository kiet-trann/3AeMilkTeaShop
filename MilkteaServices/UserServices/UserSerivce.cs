﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using MilkTea.Core.Enum;
using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkTeaRepository.UnitOfWork;
using MilkTeaWeb.ViewModels;
using System.Linq.Expressions;
using System.Text.Json;

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
            _unitOfWork.BeginTransaction();
            try
            {
                if (pageIndex < 1) pageIndex = 1;
                if (pageSize < 1) pageSize = 10;

                Expression<Func<User, bool>>? userFilter = null;
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    userFilter = u => u.FullName.Contains(filter);
                }

                var totalCount = _unitOfWork.GetRepository<User>().Count();
                var users = await _unitOfWork.GetRepository<User>().GetPaginateAsync(pageIndex, pageSize, userFilter);

                var pagedResult = new PaginatingResult<User>(users, pageIndex, totalCount, pageSize);
                _unitOfWork.CommitTransaction();
                return pagedResult;
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task<UserViewModel?> GetUserByIdAsync(int id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
                _unitOfWork.CommitTransaction();
                return user == null ? null : _mapper.Map<UserViewModel>(user);
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                return null;
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                    return null;

                var user = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Username == username);
                _unitOfWork.CommitTransaction();
                return user;
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                return null;
            }
        }

        public async Task<User?> AuthenticateAsync(LoginViewModel loginViewModel)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (string.IsNullOrWhiteSpace(loginViewModel.Username) || string.IsNullOrWhiteSpace(loginViewModel.Password))
                    return null;

                var user = await _unitOfWork.GetRepository<User>()
                                            .GetFirstOrDefaultAsync(u => u.Username == loginViewModel.Username);

                if (user == null || !user.IsActive)
                {
                    _unitOfWork.CommitTransaction();
                    return null;
                }

                _unitOfWork.CommitTransaction();
                return user.Password == loginViewModel.Password ? user : null;
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                return null;
            }
        }

        public async Task<string> AddUserAsync(string password, UserViewModel userViewModel)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (userViewModel == null)
                {
                    _unitOfWork.CommitTransaction();
                    return "Thông tin người dùng không hợp lệ";
                }

                var existingUser = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Username == userViewModel.Username);
                if (existingUser != null)
                {
                    _unitOfWork.CommitTransaction();
                    return "Tên người dùng đã tồn tại";
                }

                var user = _mapper.Map<User>(userViewModel);
                user.Password = password;

                await _unitOfWork.GetRepository<User>().AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.CommitTransaction();
                return "Thêm người dùng thành công";
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                return "Đã có lỗi xảy ra";
            }
        }

        public async Task<string> RegisterUserAsync(RegisterViewModel userViewModel)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (userViewModel == null)
                {
                    _unitOfWork.CommitTransaction();
                    return "Thông tin người dùng không hợp lệ";
                }

                var existingUser = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Username == userViewModel.Username);
                if (existingUser != null)
                {
                    _unitOfWork.CommitTransaction();
                    return "Tên người dùng đã tồn tại";
                }

                var user = _mapper.Map<User>(userViewModel);
                user.Role = "Customer";
                user.IsActive = true;

                await _unitOfWork.GetRepository<User>().AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                _unitOfWork.CommitTransaction();
                return "Đăng ký thành công";
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                return "";
            }
        }

        public async Task<string> ChangePasswordAsync(PasswordViewModel passwordViewModel)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(passwordViewModel.UserId);
                if (user == null)
                {
                    _unitOfWork.CommitTransaction();
                    return "Người dùng không tồn tại";
                }

                user.Password = passwordViewModel.Password;

                _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.SaveChangesAsync();

                _unitOfWork.CommitTransaction();
                return "Đổi mật khẩu thành công";
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                return "";
            }
        }

        public async Task<string> ToggleUserStatusAsync(int id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
                if (user == null)
                {
                    _unitOfWork.CommitTransaction();
                    return "Người dùng không tồn tại";
                }

                user.IsActive = !user.IsActive;

                _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.SaveChangesAsync();

                _unitOfWork.CommitTransaction();
                return "Cập nhật trạng thái thành công";
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                return "";
            }
        }
        public async Task<string> UpdateUserProfileAsync(UserViewModel userViewModel)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                if (userViewModel == null)
                {
                    _unitOfWork.CommitTransaction();
                    return "Thông tin người dùng không hợp lệ";
                }

                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userViewModel.UserId);
                if (user == null)
                {
                    _unitOfWork.CommitTransaction();
                    return "Người dùng không tồn tại";
                }

                string username = user.Username;
                _mapper.Map(userViewModel, user);
                user.Username = username;

                _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.CommitTransaction();

                return "Cập nhật thông tin người dùng thành công";
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                return "";
            }
        }

        public async Task<string> DeleteUserAsync(int id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
                if (user == null)
                {
                    _unitOfWork.CommitTransaction();
                    return "Người dùng không tồn tại";
                }

                _unitOfWork.GetRepository<User>().Remove(user);
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.CommitTransaction();
                return "Xóa người dùng thành công";
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                return "";
            }
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Username == username);
                _unitOfWork.CommitTransaction();
                return user == null;
            }
            catch
            {
                _unitOfWork.RollbackTransaction();
                return false;
            }
        }
    }
}
