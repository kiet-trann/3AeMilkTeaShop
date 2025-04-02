using MilkTea.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Services.ToppingServices;

namespace MilkTeaAdminWeb.Pages.Toppings
{
    public class DetailsModel : PageModel
    {
        private readonly IToppingService _toppingService;

        public DetailsModel(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        public ToppingViewModel Topping { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return RedirectToPage("./Index");
            }

            // Gọi service để lấy thông tin topping
            var toppingViewModel = await _toppingService.GetToppingByIdAsync(id.Value);

            if (toppingViewModel == null)
            {
                return NotFound();
            }

            Topping = toppingViewModel;
            return Page();
        }
    }
}
