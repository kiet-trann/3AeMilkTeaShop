using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkTea.Repository.Model;

namespace MilkTeaAdminWeb.Pages.Categories
{
    public class DetailsModel : PageModel
    {
        private readonly ThreeBrothersMilkTeaShopContext _context;

        public DetailsModel(ThreeBrothersMilkTeaShopContext context)
        {
            _context = context;
        }

        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                Category = category;
            }
            return Page();
        }
    }
}
