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
			_unitOfWork.BeginTransaction();
			try
			{
				if (pageIndex < 1) pageIndex = 1;
				if (pageSize < 1) pageSize = 10;

				var totalCount = _unitOfWork.GetRepository<Topping>().Count();

				var toppings = await _unitOfWork.GetRepository<Topping>()
					.GetPaginateAsync(pageIndex, pageSize);

				if (toppings == null || !toppings.Any())
				{
					_unitOfWork.CommitTransaction();
					return new PaginatingResult<ToppingViewModel>(Enumerable.Empty<ToppingViewModel>(), pageIndex, totalCount, pageSize);
				}

				var mappedToppings = _mapper.Map<IEnumerable<ToppingViewModel>>(toppings);

				_unitOfWork.CommitTransaction();
				return new PaginatingResult<ToppingViewModel>(mappedToppings, pageIndex, totalCount, pageSize);
			}
			catch
			{
				_unitOfWork.RollbackTransaction();
				throw;
			}
		}

		public IEnumerable<ToppingViewModel> GetAllToppings()
		{
			_unitOfWork.BeginTransaction();
			try
			{
				var toppings = _unitOfWork.GetRepository<Topping>().GetAll();
				var mappedToppings = _mapper.Map<IEnumerable<ToppingViewModel>>(toppings);
				_unitOfWork.CommitTransaction();
				return mappedToppings;
			}
			catch
			{
				_unitOfWork.RollbackTransaction();
				throw;
			}
		}

		public async Task<ToppingViewModel> GetToppingByIdAsync(int toppingId)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				if (toppingId <= 0)
					return null;

				var topping = await _unitOfWork.GetRepository<Topping>().GetByIdAsync(toppingId);
				if (topping == null)
				{
					_unitOfWork.CommitTransaction();
					return null;
				}

				_unitOfWork.CommitTransaction();
				return _mapper.Map<ToppingViewModel>(topping);
			}
			catch
			{
				_unitOfWork.RollbackTransaction();
				throw;
			}
		}

		public async Task<string> AddToppingAsync(ToppingViewModel toppingViewModel)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				if (toppingViewModel == null)
				{
					_unitOfWork.CommitTransaction();
					return "Thông tin topping không hợp lệ.";
				}

				var topping = _mapper.Map<Topping>(toppingViewModel);

				await _unitOfWork.GetRepository<Topping>().AddAsync(topping);
				await _unitOfWork.SaveChangesAsync();

				_unitOfWork.CommitTransaction();
				return "Thêm topping thành công.";
			}
			catch
			{
				_unitOfWork.RollbackTransaction();
				return "Lỗi khi thêm topping.";
			}
		}

		public async Task<string> UpdateToppingAsync(ToppingViewModel toppingViewModel)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				if (toppingViewModel == null)
				{
					_unitOfWork.CommitTransaction();
					return "Thông tin topping không hợp lệ.";
				}

				var existingTopping = await _unitOfWork.GetRepository<Topping>().GetByIdAsync(toppingViewModel.ToppingId);
				if (existingTopping == null)
				{
					_unitOfWork.CommitTransaction();
					return "Topping không tồn tại.";
				}

				_mapper.Map(toppingViewModel, existingTopping);

				_unitOfWork.GetRepository<Topping>().Update(existingTopping);
				await _unitOfWork.SaveChangesAsync();

				_unitOfWork.CommitTransaction();
				return "Cập nhật topping thành công.";
			}
			catch
			{
				_unitOfWork.RollbackTransaction();
				return "Lỗi khi cập nhật topping.";
			}
		}

		public async Task<string> DeleteToppingAsync(int toppingId)
		{
			_unitOfWork.BeginTransaction();
			try
			{
				if (toppingId <= 0)
				{
					_unitOfWork.CommitTransaction();
					return "ID topping không hợp lệ.";
				}

				var topping = await _unitOfWork.GetRepository<Topping>().GetByIdAsync(toppingId);
				if (topping == null)
				{
					_unitOfWork.CommitTransaction();
					return "Topping không tồn tại.";
				}

				_unitOfWork.GetRepository<Topping>().Remove(topping);
				await _unitOfWork.SaveChangesAsync();

				_unitOfWork.CommitTransaction();
				return "Xóa topping thành công.";
			}
			catch
			{
				_unitOfWork.RollbackTransaction();
				return "Lỗi khi xóa topping.";
			}
		}
	}
}
