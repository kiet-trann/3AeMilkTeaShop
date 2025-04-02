using MilkTea.Services.UserServices;
using MilkTea.Repository.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;

namespace MilkTeaAdminWeb.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        // Danh sách các vai trò lấy từ appsettings
        public List<string> Roles { get; set; } = new List<string>();

        public EditModel(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [BindProperty]
        public UserViewModel User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var user = await _userService.GetUserByIdAsync((int)id);
            if (user == null)
            {
                return RedirectToPage("./Index");
            }
            Roles = _configuration.GetSection("Roles").Get<List<string>>();
            User = user;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _userService.UpdateUserProfileAsync(User.UserId, User.FullName, User.PhoneNumber);

            if (result != "Cập nhật thông tin người dùng thành công")
            {
                ModelState.AddModelError(string.Empty, result);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
