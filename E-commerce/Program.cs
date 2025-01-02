using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repository;
using E_commerce.Repository.IRepository;
using E_commerce.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace E_commerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];


            builder.Services.AddDbContext<ApplicationDbContext>(
                option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });


            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IRequestCompnayRepository, RequestCompnayRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();


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
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
