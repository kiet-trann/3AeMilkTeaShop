using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Core.ViewModels;
using MilkTea.Services.ToppingServices;

namespace MilkTeaAdminWeb.Pages.Toppings
{
	public class EditModel : PageModel
	{
		private readonly IToppingService _toppingService;

		public EditModel(IToppingService toppingService)
		{
			_toppingService = toppingService;
		}

		[BindProperty]
		public ToppingViewModel ToppingViewModel { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return RedirectToPage("./Index");
			}

			var topping = await _toppingService.GetToppingByIdAsync((int)id);
			if (topping == null)
			{
				return RedirectToPage("./Index");
			}

			ToppingViewModel = topping;
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var result = await _toppingService.UpdateToppingAsync(ToppingViewModel);

			if (result != "Cập nhật topping thành công")
			{
				ModelState.AddModelError(string.Empty, result);
				return RedirectToPage("./Index");
			}

			return RedirectToPage("./Index");
		}
	}
}
