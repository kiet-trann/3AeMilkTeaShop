using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkTea.Repository.Model;

namespace MilkTeaAdminWeb.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ThreeBrothersMilkTeaShopContext _context;

        public CreateModel(MilkTea.Repository.Model.ThreeBrothersMilkTeaShopContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
