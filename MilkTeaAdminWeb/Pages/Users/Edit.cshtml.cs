using MilkTea.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Services.SignalR;

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
			if (id == null)
			{
                TempData["FailedMessage"] = "ID Không Được Trống!";
                return RedirectToPage("./Index");
			}

			var user = await _userService.GetUserByIdAsync((int)id);
			if (user == null)
			{
                TempData["SuccessMessage"] = "Người Dùng Không Tồn Tại!";
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
