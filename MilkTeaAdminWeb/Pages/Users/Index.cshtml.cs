using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Services.UserServices;
using MilkTea.Repository.Model;
using MilkTea.Core.Pagination;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace MilkTeaAdminWeb.Pages.Users
{
	public class IndexModel : PageModel
	{
		private readonly IUserService _userService;

		public IndexModel(IUserService userService)
		{
			_userService = userService;
		}

		public PaginatingResult<User> PaginatedUsers { get; set; } = default!;

		public int PageSize { get; set; } = 5;

		public string ErrorMessage { get; set; }

		public async Task<IActionResult> OnGetAsync(int pageIndex = 1)
		{
			var currentUserData = Request.Cookies["UserInfo"];

			if (!string.IsNullOrEmpty(currentUserData))
			{
				var userData = JsonSerializer.Deserialize<List<string>>(currentUserData);
				string role = userData[2];

				if (role != "Admin" && role != "Staff")
				{
					ErrorMessage = "You do not have permission to access this page.";
					return RedirectToPage("/Authentication/Login");
				}
			}
			else
			{
				ErrorMessage = "Please login first.";
				return RedirectToPage("/Authentication/Login");
			}

			PaginatedUsers = await _userService.GetPaginatedUsersAsync(pageIndex, PageSize);
			return Page();
		}
	}
}
