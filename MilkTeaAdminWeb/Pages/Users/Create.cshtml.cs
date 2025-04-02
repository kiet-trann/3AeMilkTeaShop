using MilkTea.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Services.UserServices;

namespace MilkTeaAdminWeb.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        // List role lấy trong appsettings
        public List<string> Roles { get; set; } = new List<string>();

        public CreateModel(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public void OnGet()
        {
            Roles = _configuration.GetSection("Roles").Get<List<string>>();
        }

        [BindProperty]
        public UserViewModel UserRequestModel { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _userService.AddUserAsync(UserRequestModel);

            if (result != "Thêm người dùng thành công")
            {
                ModelState.AddModelError(string.Empty, result);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
