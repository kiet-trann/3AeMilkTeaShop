using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Core.ViewModels;
using MilkTea.Services.SignalR;
using MilkteaServices.CategoryServices;

namespace MilkTeaAdminWeb.Pages.Categories
{
	public class EditModel : PageModel
	{
		private readonly ICategoryService _categoryService;
		private readonly IHubContext<SignalHub> _hubContext;

		public EditModel(ICategoryService categoryService, IHubContext<SignalHub> hubContext)
		{
			_categoryService = categoryService;
			_hubContext = hubContext;
		}

		[BindProperty]
		public CategoryViewModel CategoryViewModel { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				TempData["FailedMessage"] = "ID Không Được Trống!";
				return RedirectToPage("./Index");
			}

			var category = await _categoryService.GetCategoryByIdAsync((int)id);
			if (category == null)
			{
				TempData["FailedMessage"] = "Danh mục không có sẵn!";
				return RedirectToPage("./Index");
			}

			CategoryViewModel = category;
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var result = await _categoryService.UpdateCategoryAsync(CategoryViewModel);

			if (result == "Cập nhật danh mục thành công")
			{
				TempData["SuccessMessage"] = result;
				await _hubContext.Clients.All.SendAsync("LoadPage", "Categories");
				return RedirectToPage("./Index");
			}

			TempData["FailedMessage"] = result;
			return RedirectToPage("./Index");
		}
	}
}
