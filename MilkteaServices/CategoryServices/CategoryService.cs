using AutoMapper;
using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkTeaRepository.UnitOfWork;

namespace MilkteaServices.CategoryServices
{
	public class CategoryService : ICategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<PaginatingResult<Category>> GetPaginatedCategoriesAsync(int pageIndex, int pageSize)
		{
			_unitOfWork.BeginTransaction();
			if (pageIndex < 1) pageIndex = 1;
			if (pageSize < 1) pageSize = 10;

			var totalCount = _unitOfWork.GetRepository<Category>().Count();
			var categories = await _unitOfWork.GetRepository<Category>().GetPaginateAsync(pageIndex, pageSize);
			var pagedResult = new PaginatingResult<Category>(categories, pageIndex, totalCount, pageSize);

			_unitOfWork.CommitTransaction();
			return pagedResult;
		}

		public IEnumerable<CategoryViewModel> GetAvailableCategories()
		{
			_unitOfWork.BeginTransaction();

			var categories = _unitOfWork.GetRepository<Category>()
				.GetAll(c => c.IsActive == true);

			_unitOfWork.CommitTransaction();
			return _mapper.Map<IEnumerable<CategoryViewModel>>(categories);

		}

		public async Task<CategoryViewModel?> GetCategoryByIdAsync(int categoryId)
		{
			_unitOfWork.BeginTransaction();

			var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId);
			_unitOfWork.CommitTransaction();
			return category == null ? null : _mapper.Map<CategoryViewModel>(category);
		}

		public async Task<string> AddCategoryAsync(CategoryViewModel categoryViewModel)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				if (categoryViewModel == null)
					return "Dữ liệu danh mục không hợp lệ";

				var existingCategory = await _unitOfWork.GetRepository<Category>()
					.GetFirstOrDefaultAsync(c => c.CategoryName == categoryViewModel.CategoryName);

				if (existingCategory != null)
					return "Danh mục đã tồn tại";

				var category = _mapper.Map<Category>(categoryViewModel);
				await _unitOfWork.GetRepository<Category>().AddAsync(category);
				await _unitOfWork.SaveChangesAsync();

				_unitOfWork.CommitTransaction();
				return "Thêm danh mục thành công";
			}
			catch (Exception)
			{
				_unitOfWork.RollbackTransaction();
				return "Đã xảy ra lỗi khi thêm danh mục";
			}
		}

		public async Task<string> UpdateCategoryAsync(CategoryViewModel categoryViewModel)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				if (categoryViewModel == null)
					return "Dữ liệu danh mục không hợp lệ";

				var existingCategory = await _unitOfWork.GetRepository<Category>()
					.GetByIdAsync(categoryViewModel.CategoryId);

				if (existingCategory == null)
					return "Danh mục không tồn tại";

				_mapper.Map(categoryViewModel, existingCategory);
				_unitOfWork.GetRepository<Category>().Update(existingCategory);
				await _unitOfWork.SaveChangesAsync();

				_unitOfWork.CommitTransaction();
				return "Cập nhật danh mục thành công";
			}
			catch (Exception)
			{
				_unitOfWork.RollbackTransaction();
				return "Đã xảy ra lỗi khi cập nhật danh mục";
			}
		}

		public async Task<string> DeleteCategoryAsync(int categoryId)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId);

				if (category == null)
					return "Danh mục không tồn tại";

				_unitOfWork.GetRepository<Category>().Remove(category);
				await _unitOfWork.SaveChangesAsync();

				_unitOfWork.CommitTransaction();
				return "Xóa danh mục thành công";
			}
			catch (Exception)
			{
				_unitOfWork.RollbackTransaction();
				return "Đã xảy ra lỗi khi xóa danh mục";
			}
		}
	}
}
