using MilkTea.Services.UserServices;
using MilkTea.Repository.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using System.Text.Json;

namespace MilkTeaAdminWeb.Pages.Users
{
	public class DetailsModel : PageModel
	{
		private readonly IUserService _userService;

		public DetailsModel(IUserService userService)
		{
			_userService = userService;
		}

		public UserViewModel User { get; set; } = default!;
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
				ErrorMessage = "You do not have permission to access this page.";
				return RedirectToPage("/Authentication/Login");
			}

			if (id == null)
			{
				return RedirectToPage("./Index");
			}

			var user = await _userService.GetUserByIdAsync((int)id);
			if (user == null)
			{
				return RedirectToPage("./Index");
			}
			else
			{
				User = user;
			}
			return Page();
		}
	}
}
