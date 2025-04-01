using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkTea.Repository.Model;

namespace MilkTeaAdminWeb.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly ThreeBrothersMilkTeaShopContext _context;

        public IndexModel(ThreeBrothersMilkTeaShopContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Category = await _context.Categories.ToListAsync();
        }
    }
}
