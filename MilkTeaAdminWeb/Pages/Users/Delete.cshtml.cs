using MilkTea.Services.UserServices;
using MilkTea.Repository.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MilkTeaAdminWeb.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly IUserService _userService;

        public DeleteModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Tải thông tin người dùng để hiển thị trên trang
            await _userService.GetUserByIdAsync(id.Value);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            // Gọi phương thức xóa người dùng từ service
            var result = await _userService.DeleteUserAsync(id.Value);

            if (result != "Xóa người dùng thành công")
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
