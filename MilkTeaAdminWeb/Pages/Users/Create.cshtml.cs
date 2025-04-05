using MilkTea.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Services.UserServices;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Services.SignalR;
using System.Text.Json;

namespace MilkTeaAdminWeb.Pages.Users
{
	public class CreateModel : PageModel
	{
		private readonly IUserService _userService;
		private readonly IConfiguration _configuration;
		private readonly IHubContext<SignalHub> _hubContext;

		public List<string> Roles { get; set; } = new List<string>();

		public CreateModel(IUserService userService, IConfiguration configuration, IHubContext<SignalHub> hubContext)
		{
			_userService = userService;
			_configuration = configuration;
			_hubContext = hubContext;
		}

		public IActionResult OnGet()
		{
			var currentUserData = Request.Cookies["UserInfo"];

			if (string.IsNullOrEmpty(currentUserData))
			{
				TempData["FailedMessage"] = "Please login first.";
				return RedirectToPage("/Authentication/Login");
			}

			var userData = JsonSerializer.Deserialize<List<string>>(currentUserData);
			string role = userData[2];

			if (role != "Admin")
			{
				TempData["FailedMessage"] = "You do not have permission to create users.";
				return RedirectToPage("/Authentication/Login");
			}

			Roles = _configuration.GetSection("Roles").Get<List<string>>();
			return Page();
		}

		[BindProperty]
		public UserViewModel UserRequestModel { get; set; } = default!;

		[BindProperty]
		public string Password { get; set; } = default!;

		public async Task<IActionResult> OnPostAsync()
		{
			var currentUserData = Request.Cookies["UserInfo"];

			if (string.IsNullOrEmpty(currentUserData))
			{
				TempData["FailedMessage"] = "Please login first.";
				return RedirectToPage("/Authentication/Login");
			}

			var userData = JsonSerializer.Deserialize<List<string>>(currentUserData);
			string role = userData[2];

			if (role != "Admin")
			{
				TempData["FailedMessage"] = "You do not have permission to create users.";
				return RedirectToPage("/Authentication/Login");
			}

			var result = await _userService.AddUserAsync(Password, UserRequestModel);

			if (result == "Thêm người dùng thành công")
			{
				TempData["SuccessMessage"] = result;
				await _hubContext.Clients.All.SendAsync("LoadPage", "Users");
				return RedirectToPage("./Index");
			}

			TempData["FailedMessage"] = result;
			return Page();
		}
	}
}
