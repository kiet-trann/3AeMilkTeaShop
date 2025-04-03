using AutoMapper;
using MilkTea.Core.Pagination;
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

		public async Task<PaginatingResult<ProductViewModel>> GetPaginatedProductsAsync(int pageIndex, int pageSize)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				if (pageIndex < 1) pageIndex = 1;
				if (pageSize < 1) pageSize = 10;

				var totalCount = _unitOfWork.GetRepository<Product>().Count();
				var products = await _unitOfWork.GetRepository<Product>()
					.GetPaginateAsync(pageIndex, pageSize, null, null, "Category");

				var productViewModels = products.Select(product =>
				{
					var productVM = _mapper.Map<ProductViewModel>(product);
					productVM.Category = _mapper.Map<CategoryViewModel>(product.Category);
					return productVM;
				}).ToList();

				_unitOfWork.CommitTransaction();
				return new PaginatingResult<ProductViewModel>(productViewModels, pageIndex, totalCount, pageSize);
			}
			catch
			{
				_unitOfWork.RollbackTransaction();
				throw;
			}
		}

		public async Task<PaginatingResult<ProductViewModel>> GetProductByCategory(int categoryId, int pageNumber, int pageSize)
		{
			if (pageNumber < 1) pageNumber = 1;
			if (pageSize < 1) pageSize = 10;

			var totalCount = _unitOfWork.GetRepository<Product>().Count();

			var products = await _unitOfWork.GetRepository<Product>()
				.GetPaginateAsync(pageNumber, pageSize, p => p.CategoryId == categoryId, null, "Category");

			var productViewModels = products.Select(product =>
			{
				var productVM = _mapper.Map<ProductViewModel>(product);
				productVM.Category = _mapper.Map<CategoryViewModel>(product.Category);
				return productVM;
			}).ToList();

			return new PaginatingResult<ProductViewModel>(productViewModels, pageNumber, totalCount, pageSize);
		}

		public async Task<ProductViewModel> GetProductByIdAsync(int productId)
		{
			if (productId <= 0)
				return null;

			var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
			if (product == null)
				return null;

			return _mapper.Map<ProductViewModel>(product);
		}

		public async Task<string> AddProductAsync(ProductViewModel productViewModel)
		{
			if (productViewModel == null)
				return "Thông tin sản phẩm không hợp lệ.";

			var product = _mapper.Map<Product>(productViewModel);

			try
			{
				_unitOfWork.BeginTransaction();

				await _unitOfWork.GetRepository<Product>().AddAsync(product);
				await _unitOfWork.SaveChangesAsync();

				_unitOfWork.CommitTransaction();
			}
			catch (Exception)
			{
				_unitOfWork.RollbackTransaction();
				return "Đã có lỗi xảy ra khi thêm sản phẩm.";
			}

			return "Thêm sản phẩm thành công.";
		}

		public async Task<string> UpdateProductAsync(ProductViewModel productViewModel)
		{
			if (productViewModel == null)
				return "Thông tin sản phẩm không hợp lệ.";

			var existingProduct = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productViewModel.ProductId);
			if (existingProduct == null)
				return "Sản phẩm không tồn tại.";

			try
			{
				_unitOfWork.BeginTransaction();

				_mapper.Map(productViewModel, existingProduct);
				_unitOfWork.GetRepository<Product>().Update(existingProduct);
				await _unitOfWork.SaveChangesAsync();

				_unitOfWork.CommitTransaction();
			}
			catch (Exception)
			{
				_unitOfWork.RollbackTransaction();
				return "Đã có lỗi xảy ra khi cập nhật sản phẩm.";
			}

			return "Cập nhật sản phẩm thành công!";
		}

		public async Task<string> DeleteProductAsync(int productId)
		{
			try
			{
				_unitOfWork.BeginTransaction();

				if (productId <= 0)
					return "ID sản phẩm không hợp lệ.";

				var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
				if (product == null)
					return "Sản phẩm không tồn tại.";

				_unitOfWork.GetRepository<Product>().Remove(productId);
				await _unitOfWork.SaveChangesAsync();

				_unitOfWork.CommitTransaction();
			}
			catch (Exception)
			{
				_unitOfWork.RollbackTransaction();
				return "Đã có lỗi xảy ra khi xóa sản phẩm.";
			}

			return "Xóa sản phẩm thành công.";
		}
	}
}
