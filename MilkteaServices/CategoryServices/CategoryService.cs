using AutoMapper;
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

		public async Task<IEnumerable<Category>> GetPaginatedCategoriesAsync(int pageIndex, int pageSize)
		{
			if (pageIndex < 1) pageIndex = 1;
			if (pageSize < 1) pageSize = 10;

			return await _unitOfWork.GetRepository<Category>().GetPaginateAsync(pageIndex, pageSize);
		}

		public async Task<string> AddCategoryAsync(CategoryViewModel categoryViewModel)
		{
			if (categoryViewModel == null)
			{
				return "Dữ liệu danh mục không hợp lệ";
			}

			// Kiểm tra nếu danh mục đã tồn tại
			var existingCategory = await _unitOfWork.GetRepository<Category>()
													.GetFirstOrDefaultAsync(c => c.CategoryName == categoryViewModel.CategoryName);
			if (existingCategory != null)
			{
				return "Danh mục đã tồn tại";
			}

			var category = _mapper.Map<Category>(categoryViewModel);

			await _unitOfWork.GetRepository<Category>().AddAsync(category);
			await _unitOfWork.SaveChangesAsync();
			return "Thêm danh mục thành công";
		}

		public async Task<string> UpdateCategoryAsync(string categoryId, CategoryViewModel categoryViewModel)
		{
			if (categoryViewModel == null)
			{
				return "Dữ liệu danh mục không hợp lệ";
			}

			var existingCategory = await _unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId);
			if (existingCategory == null)
			{
				return "Danh mục không tồn tại";
			}

			_mapper.Map(categoryViewModel, existingCategory);

			await _unitOfWork.GetRepository<Category>().UpdateAsync(existingCategory);
			await _unitOfWork.SaveChangesAsync();
			return "Cập nhật danh mục thành công";
		}

		public async Task<string> DeleteCategoryAsync(int categoryId)
		{
			var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId);
			if (category == null)
			{
				return "Danh mục không tồn tại";
			}

			await _unitOfWork.GetRepository<Category>().RemoveAsync(category);
			await _unitOfWork.SaveChangesAsync();
			return "Xóa danh mục thành công";
		}
	}
}
