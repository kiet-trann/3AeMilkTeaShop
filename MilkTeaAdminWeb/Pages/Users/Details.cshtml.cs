using MilkTea.Services.UserServices;
using MilkTea.Repository.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;

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
            else
            {
                User = user;
            }
            return Page();
        }
    }
}
