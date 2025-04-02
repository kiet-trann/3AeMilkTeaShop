using Microsoft.EntityFrameworkCore;
using MilkTea.Repository.Mapping;
using MilkTea.Repository.Model;
using MilkTea.Services.UserServices;
using MilkTeaRepository.GenericRepository;
using MilkTeaRepository.UnitOfWork;
using MilkteaServices.CategoryServices;

namespace MilkTeaAdminWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Đăng ký dịch vụ trước khi gọi builder.Build()
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<ThreeBrothersMilkTeaShopContext>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(MappingProfile)); 

            var app = builder.Build(); 

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
