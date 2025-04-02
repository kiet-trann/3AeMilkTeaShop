using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using MilkTea.Services.UserServices;

namespace MilkTeaAdminWeb.Pages.Users
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserService _userService;

        public ChangePasswordModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public PasswordViewModel PasswordViewModel { get; set; } = new PasswordViewModel();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            PasswordViewModel.UserId = (int)id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _userService.ChangePasswordAsync(PasswordViewModel);

            if (result == "Đổi mật khẩu thành công")
            {
                TempData["SuccessMessage"] = result;
                return RedirectToPage("/Users/Index");
            }

            return Page();
        }
    }
}
