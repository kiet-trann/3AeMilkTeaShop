using MilkTea.Services.UserServices;
using MilkTea.Repository.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Services.SignalR;
using MilkTea.Core.ViewModels;
using System.Text.Json;

namespace MilkTeaAdminWeb.Pages.Users
{
	public class DeleteModel : PageModel
	{
		private readonly IUserService _userService;
		private readonly IHubContext<SignalHub> _hubContext;

		public DeleteModel(IUserService userService, IHubContext<SignalHub> hubContext)
		{
			_userService = userService;
			_hubContext = hubContext;
		}

		[BindProperty]
		public UserViewModel UserViewModel { get; set; } = default!;

		public string ErrorMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			var currentUserData = Request.Cookies["UserInfo"];

			if (string.IsNullOrEmpty(currentUserData))
			{
				ErrorMessage = "Please login first.";
				return RedirectToPage("/Authentication/Login");
			}

			var userData = JsonSerializer.Deserialize<List<string>>(currentUserData);
			string role = userData[2];

			if (role != "Admin")
			{
				ErrorMessage = "You do not have permission to delete users.";
				return RedirectToPage("/Authentication/Login");
			}

			if (id == null)
			{
				TempData["FailedMessage"] = "ID Không Được Trống!";
				return RedirectToPage("./Index");
			}

			UserViewModel = await _userService.GetUserByIdAsync(id.Value);

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			var currentUserData = Request.Cookies["UserInfo"];

			if (string.IsNullOrEmpty(currentUserData))
			{
				ErrorMessage = "Please login first.";
				return RedirectToPage("/Authentication/Login");
			}

			var userData = JsonSerializer.Deserialize<List<string>>(currentUserData);
			string role = userData[2];

			if (role != "Admin")
			{
				ErrorMessage = "You do not have permission to delete users.";
				return RedirectToPage("/Authentication/Login");
			}

			if (id == null)
			{
				TempData["FailedMessage"] = "ID Không Được Trống!";
				return RedirectToPage("./Index");
			}

			var result = await _userService.DeleteUserAsync(id.Value);

			if (result == "Xóa người dùng thành công")
			{
				TempData["SuccessMessage"] = result;
				await _hubContext.Clients.All.SendAsync("LoadPage", "Users");
				return RedirectToPage("./Index");
			}

			TempData["FailedMessage"] = result;
			return RedirectToPage("./Index");
		}
	}
}
