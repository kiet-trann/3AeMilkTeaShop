using AutoMapper;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkTeaRepository.UnitOfWork;

namespace MilkTea.Services.ProductServices
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		// Lấy danh sách sản phẩm có phân trang
		public async Task<IEnumerable<ProductViewModel>> GetPaginatedProductsAsync(int pageIndex, int pageSize)
		{
			if (pageIndex < 1) pageIndex = 1;
			if (pageSize < 1) pageSize = 10;

			var products = await _unitOfWork.GetRepository<Product>().GetPaginateAsync(pageIndex, pageSize);
			if (products == null || !products.Any())
			{
				return Enumerable.Empty<ProductViewModel>();
			}

			// Ánh xạ từ sản phẩm sang ProductViewModel
			return _mapper.Map<IEnumerable<ProductViewModel>>(products);
		}

		// Lấy thông tin sản phẩm theo ID
		public async Task<ProductViewModel> GetProductByIdAsync(int productId)
		{
			if (productId <= 0)
				return null;

			var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
			if (product == null)
				return null;

			// Ánh xạ từ Product sang ProductViewModel
			return _mapper.Map<ProductViewModel>(product);
		}

		// Thêm sản phẩm mới
		public async Task<string> AddProductAsync(ProductViewModel productViewModel)
		{
			if (productViewModel == null)
				return "Thông tin sản phẩm không hợp lệ.";

			// Ánh xạ từ ProductViewModel sang Product entity
			var product = _mapper.Map<Product>(productViewModel);

			await _unitOfWork.GetRepository<Product>().AddAsync(product);
			await _unitOfWork.SaveChangesAsync();

			return "Thêm sản phẩm thành công.";
		}

		// Cập nhật thông tin sản phẩm
		public async Task<string> UpdateProductAsync(int productId, ProductViewModel productViewModel)
		{
			if (productViewModel == null)
				return "Thông tin sản phẩm không hợp lệ.";

			var existingProduct = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
			if (existingProduct == null)
				return "Sản phẩm không tồn tại.";

			// Ánh xạ từ ProductViewModel vào entity Product hiện có
			_mapper.Map(productViewModel, existingProduct);

			await _unitOfWork.GetRepository<Product>().UpdateAsync(existingProduct);
			await _unitOfWork.SaveChangesAsync();

			return "Cập nhật sản phẩm thành công.";
		}

		// Xóa sản phẩm theo ID
		public async Task<string> DeleteProductAsync(int productId)
		{
			if (productId <= 0)
				return "ID sản phẩm không hợp lệ.";

			var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
			if (product == null)
				return "Sản phẩm không tồn tại.";

			await _unitOfWork.GetRepository<Product>().RemoveAsync(product);
			await _unitOfWork.SaveChangesAsync();

			return "Xóa sản phẩm thành công.";
		}
	}
}
