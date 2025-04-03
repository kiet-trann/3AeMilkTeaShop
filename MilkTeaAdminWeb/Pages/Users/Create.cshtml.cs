using MilkTea.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Services.UserServices;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Services.SignalR;

namespace MilkTeaAdminWeb.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<SignalHub> _hubContext;

        // List role lấy trong appsettings
        public List<string> Roles { get; set; } = new List<string>();

        public CreateModel(IUserService userService, IConfiguration configuration, IHubContext<SignalHub> hubContext)
        {
            _userService = userService;
            _configuration = configuration;
            _hubContext = hubContext;
        }

        public void OnGet()
        {
            Roles = _configuration.GetSection("Roles").Get<List<string>>();
        }

        [BindProperty]
        public UserViewModel UserRequestModel { get; set; } = default!;
		[BindProperty]
		public string Password { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _userService.AddUserAsync(Password, UserRequestModel);

            if (result == "Thêm người dùng thành công")
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
