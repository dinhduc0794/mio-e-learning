using Microsoft.EntityFrameworkCore;
using Mio.ELearning.AdminSite.Models;
using Mio.ELearning.Core.Repositories;
using Mio.ELearning.Core.UnitOfWorks;
using Mio.ELearning.Data;
using Mio.ELearning.Data.Repositories;
using Mio.ELearning.Service.Services;
using Mio.ELearning.Service.Services.Impl;

namespace Mio.ELearning.AdminSite;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);   //tao builder de cau hinh ung dung
        
        // Lấy chuỗi kết nối từ file appsettings.json → dùng để kết nối database.
        // Cấu hình môi trường Dev: setx ASPNETCORE_ENVIRONMENT "Development"
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        
        if (connectionString == string.Empty)
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
        
        // Add services to the container.
        builder.Services.AddDbContext<LMSDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            )
        );
        
        builder.Services.Configure<IISServerOptions>(options =>
        {
            options.MaxRequestBodySize = 104857600; // 100 MB
        });
        
        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //type of dung de dang ki generic 
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ICourseRepository, CourseRepository>();
        builder.Services.AddScoped<ISectionRepository, SectionRepository>();
        builder.Services.AddScoped<ILessonRepository, LessonRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ICourseService, CourseService>();
        builder.Services.AddScoped<ISectionService, SectionService>();
        builder.Services.AddScoped<ILessonService, LessonService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
    
        // app.MapControllerRoute(
        //     name: "mycourse",
        //     pattern: "MyCourse/{controller=Course}/{action=Index}/{id?}");
        
        // app.MapControllerRoute(
        //     name: "areas",
        //     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
        
        // /Areas
        //     /MyCourse
        //        /Controllers
        //          CourseController.cs
        //        /Views
        //          /Course
        //              Index.cshtml
        app.Run();
    }
}