using AutoMapper;
using MilkTea.Core.ViewModels;
using MilkTea.Repository.Model;
using MilkTeaWeb.ViewModels;

namespace MilkTea.Repository.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User
            CreateMap<RegisterViewModel, User>();
            CreateMap<UserViewModel, User>();
            CreateMap<User, UserViewModel>();
            CreateMap<LoginViewModel, User>();
          //  CreateMap<User, UserBasicViewModel>();

            // Topping
            CreateMap<ToppingViewModel, Topping>();
            CreateMap<Topping, ToppingViewModel>();

            // Category
            CreateMap<CategoryViewModel, Category>();
            CreateMap<Category, CategoryViewModel>();

            //Product
            CreateMap<ProductViewModel, Product>();
            CreateMap<Product, ProductViewModel>();
            //orrder
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderDetail, OrderDetailViewModel>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));

        

			//Product
			CreateMap<OrderViewModel, Product>();
			CreateMap<Order, OrderViewModel>();
		}
    }
}
