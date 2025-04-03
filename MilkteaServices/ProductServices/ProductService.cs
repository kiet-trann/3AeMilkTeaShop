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
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;

            var totalCount = await _unitOfWork.GetRepository<Product>().CountAsync();

            // Lấy danh sách sản phẩm kèm theo Category
            var products = await _unitOfWork.GetRepository<Product>()
                .GetPaginateAsync(pageIndex, pageSize, null, null, "Category"); 

            // Ánh xạ dữ liệu
            var productViewModels = products.Select(product =>
            {
                var productVM = _mapper.Map<ProductViewModel>(product);
                productVM.Category = _mapper.Map<CategoryViewModel>(product.Category);
                return productVM;
            }).ToList();

            var pagedResult = new PaginatingResult<ProductViewModel>(
                productViewModels,
                pageIndex,
                totalCount,
                pageSize
            );

            return pagedResult;
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

            await _unitOfWork.GetRepository<Product>().AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return "Thêm sản phẩm thành công.";
        }

        public async Task<string> UpdateProductAsync(ProductViewModel productViewModel)
        {
            if (productViewModel == null)
                return "Thông tin sản phẩm không hợp lệ.";

            var existingProduct = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productViewModel.ProductId);
            if (existingProduct == null)
                return "Sản phẩm không tồn tại.";

            _mapper.Map(productViewModel, existingProduct);

            await _unitOfWork.GetRepository<Product>().UpdateAsync(existingProduct);
            await _unitOfWork.SaveChangesAsync();

            return "Cập nhật sản phẩm thành công.";
        }

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
