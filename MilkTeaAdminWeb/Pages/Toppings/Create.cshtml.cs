using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using MilkTea.Services.ToppingServices;

namespace MilkTeaAdminWeb.Pages.Toppings
{
    public class CreateModel : PageModel
    {
        private readonly IToppingService _toppingService;

        public CreateModel(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        [BindProperty]
        public ToppingViewModel ToppingRequestModel { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _toppingService.AddToppingAsync(ToppingRequestModel);

            if (result != "Thêm topping thành công")
            {
                ModelState.AddModelError(string.Empty, result);
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
