using MilkTea.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Services.SignalR;
using System.Text.Json;

namespace MilkTeaAdminWeb.Pages.Users
{
	public class EditModel : PageModel
	{
		private readonly IUserService _userService;
		private readonly IConfiguration _configuration;
		private readonly IHubContext<SignalHub> _hubContext;

		// Role lấy từ appsettings
		public List<string> Roles { get; set; } = new List<string>();

		public EditModel(IUserService userService, IConfiguration configuration, IHubContext<SignalHub> hubContext)
		{
			_userService = userService;
			_configuration = configuration;
			_hubContext = hubContext;
		}

		[BindProperty]
		public UserViewModel UserViewModel { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			var currentUserData = Request.Cookies["UserInfo"];
			if (!string.IsNullOrEmpty(currentUserData))
			{
				var userData = JsonSerializer.Deserialize<List<string>>(currentUserData);
				string role = userData[2]; // Lấy Role

				if (role != "Admin" && role != "Staff")
				{
					TempData["FailedMessage"] = "Bạn không có quyền truy cập vào trang này!";
					return RedirectToPage("/Authentication/Login");
				}
			}
			else
			{
				TempData["FailedMessage"] = "Vui lòng đăng nhập trước!";
				return RedirectToPage("/Authentication/Login");
			}

			if (id == null)
			{
				TempData["FailedMessage"] = "ID Không Được Trống!";
				return RedirectToPage("./Index");
			}

			var user = await _userService.GetUserByIdAsync((int)id);
			if (user == null)
			{
				TempData["SuccessMessage"] = "Người Dùng Không Tồn Tại!";
				return RedirectToPage("./Index");
			}

			Roles = _configuration.GetSection("Roles").Get<List<string>>();
			UserViewModel = user;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var result = await _userService.UpdateUserProfileAsync(UserViewModel);

			if (result == "Cập nhật thông tin người dùng thành công")
			{
				TempData["SuccessMessage"] = result;
				await _hubContext.Clients.All.SendAsync("LoadPage", "Toppings");
				return RedirectToPage("./Index");
			}

			TempData["FailedMessage"] = result;
			return RedirectToPage("./Index");
		}
	}
}
