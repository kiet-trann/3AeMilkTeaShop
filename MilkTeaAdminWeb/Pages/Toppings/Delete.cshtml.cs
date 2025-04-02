using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkTea.Services.ToppingServices;

namespace MilkTeaAdminWeb.Pages.Toppings
{
    public class DeleteModel : PageModel
    {
        private readonly IToppingService _toppingService;

        public DeleteModel(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        [BindProperty]
        public Topping Topping { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Lấy thông tin topping
            var topping = await _toppingService.GetToppingByIdAsync(id.Value);

            if (topping == null)
            {
                return NotFound();
            }
            else
            {
                Topping = new Topping
                {
                    ToppingId = topping.ToppingId,
                    ToppingName = topping.ToppingName,
                    Price = topping.Price,
                    IsAvailable = topping.IsAvailable
                };
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            var result = await _toppingService.DeleteToppingAsync(id.Value);

            if (result == "Xóa topping thành công")
            {
                ViewData["Message"] = result;
            }
            else
            {
                ViewData["Message"] = result;
            }

            return RedirectToPage("./Index");
        }
    }
}
