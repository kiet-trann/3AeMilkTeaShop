using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MilkTea.Core.ViewModels;
using MilkTea.Services.SignalR;
using MilkteaServices.CategoryServices;

namespace MilkTeaAdminWeb.Pages.Categories
{
	public class CreateModel : PageModel
	{
		private readonly ICategoryService _categoryService;
		private readonly IHubContext<SignalHub> _hubContext;
		public CreateModel(ICategoryService categoryService, IHubContext<SignalHub> hubContext)
		{
			_categoryService = categoryService;
			_hubContext = hubContext;
		}

		[BindProperty]
		public CategoryViewModel CategoryRequestModel { get; set; } = default!;

		public IActionResult OnGet()
		{
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var result = await _categoryService.AddCategoryAsync(CategoryRequestModel);

			if (result == "Thêm danh mục thành công")
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
