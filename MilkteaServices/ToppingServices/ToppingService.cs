using AutoMapper;
using MilkTea.Core.Pagination;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkTeaRepository.UnitOfWork;

namespace MilkTea.Services.ToppingServices
{
    public class ToppingService : IToppingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ToppingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatingResult<ToppingViewModel>> GetPaginatedToppingsAsync(int pageIndex, int pageSize)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;

            var totalCount = await _unitOfWork.GetRepository<Topping>().CountAsync();

            var toppings = await _unitOfWork.GetRepository<Topping>()
                .GetPaginateAsync(pageIndex, pageSize);

            if (toppings == null || !toppings.Any())
            {
                return new PaginatingResult<ToppingViewModel>(Enumerable.Empty<ToppingViewModel>(), pageIndex, totalCount, pageSize);
            }

            var mappedToppings = _mapper.Map<IEnumerable<ToppingViewModel>>(toppings);

            return new PaginatingResult<ToppingViewModel>(mappedToppings, pageIndex, totalCount, pageSize);
        }
		
		public async Task<IEnumerable<ToppingViewModel>> GetAllToppingsAsync()
		{
			var toppings = await _unitOfWork.GetRepository<Topping>().GetAllAsync();
			return _mapper.Map<IEnumerable<ToppingViewModel>>(toppings);
		}
		public async Task<ToppingViewModel> GetToppingByIdAsync(int toppingId)
        {
            if (toppingId <= 0)
                return null;

            var topping = await _unitOfWork.GetRepository<Topping>().GetByIdAsync(toppingId);
            if (topping == null)
                return null;

            return _mapper.Map<ToppingViewModel>(topping);
        }

        public async Task<string> AddToppingAsync(ToppingViewModel toppingViewModel)
        {
            if (toppingViewModel == null)
                return "Thông tin topping không hợp lệ.";

            var topping = _mapper.Map<Topping>(toppingViewModel);

            await _unitOfWork.GetRepository<Topping>().AddAsync(topping);
            await _unitOfWork.SaveChangesAsync();

            return "Thêm topping thành công.";
        }

        public async Task<string> UpdateToppingAsync(ToppingViewModel toppingViewModel)
        {
            if (toppingViewModel == null)
                return "Thông tin topping không hợp lệ.";

            var existingTopping = await _unitOfWork.GetRepository<Topping>().GetByIdAsync(toppingViewModel.ToppingId);
            if (existingTopping == null)
                return "Topping không tồn tại.";

            _mapper.Map(toppingViewModel, existingTopping);

            await _unitOfWork.GetRepository<Topping>().UpdateAsync(existingTopping);
            await _unitOfWork.SaveChangesAsync();

            return "Cập nhật topping thành công.";
        }

        public async Task<string> DeleteToppingAsync(int toppingId)
        {
            if (toppingId <= 0)
                return "ID topping không hợp lệ.";

            var topping = await _unitOfWork.GetRepository<Topping>().GetByIdAsync(toppingId);
            if (topping == null)
                return "Topping không tồn tại.";

            await _unitOfWork.GetRepository<Topping>().RemoveAsync(topping);
            await _unitOfWork.SaveChangesAsync();

            return "Xóa topping thành công.";
        }
    }
}
