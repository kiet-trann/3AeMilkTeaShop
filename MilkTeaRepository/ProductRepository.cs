using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkTeaRepository.GenericRepository;

namespace MilkTea.Repository
{
	public class ProductRepository : GenericRepository<Product>
	{
		private readonly IMapper _mapper;
		public ProductRepository(ThreeBrothersMilkTeaShopContext context, IMapper mapper) : base(context)
		{
			_mapper = mapper;
		}

		public async Task<PaginatingResult<ProductViewModel>> GetProductByCategory(int categoryId, int pageNumber, int pageSize)
		{
			var query = _context.Products
								.Where(p => p.CategoryId == categoryId)
								.AsQueryable();

			var totalCount = await query.CountAsync();

			var products = await query
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			var productViewModels = _mapper.Map<List<ProductViewModel>>(products);

			return new PaginatingResult<ProductViewModel>(productViewModels, pageNumber, totalCount, pageSize);
		}

	}
}
