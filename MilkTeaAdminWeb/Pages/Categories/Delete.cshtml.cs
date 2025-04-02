using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkteaServices.CategoryServices;
using MilkTea.Repository.Model;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Services.SignalR;
using MilkTea.Core.ViewModels;

namespace MilkTeaAdminWeb.Pages.Categories
{
	public class DeleteModel : PageModel
	{
		private readonly ICategoryService _categoryService;
		private readonly IHubContext<SignalHub> _hubContext;

		public DeleteModel(ICategoryService categoryService, IHubContext<SignalHub> hubContext)
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
				return RedirectToPage("./Index");
			}

			var category = await _categoryService.GetCategoryByIdAsync((int)id);

			if (category == null)
			{
				return RedirectToPage("./Index");
			}

			CategoryViewModel = category;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return RedirectToPage("./Index");
			}

			var result = await _categoryService.DeleteCategoryAsync(id.Value);

			if (result == "Xóa danh mục thành công")
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
