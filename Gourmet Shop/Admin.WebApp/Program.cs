using Admin.WebApp.Data;
using GourmetShop.DataAccess;
using GourmetShop.DataAccess.Data;
using GourmetShop.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Admin.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "net9.0", "ConnectionString.txt"));
            builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"] = connectionString.Replace(@"\", @"\\");
            // Add services to the container.

            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<GourmetShopDbContext>(options => options .UseSqlServer(builder.Configuration
                .GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("GourmetShop.DataAccess"))
                .UseSeeding((context, _) =>
                {
                    DataInitializer dataInitializer = new DataInitializer();
                    dataInitializer.Initialize((GourmetShopDbContext)context);
                })
               );

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<Authentication, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            }).AddEntityFrameworkStores<GourmetShopDbContext>().AddDefaultTokenProviders().AddRoles<IdentityRole>();
            
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<GourmetShopDbContext>();
                try
                {
                    DataInitializer dataInitializer = new DataInitializer();
                    dataInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            
            

            app.Run();
        }
    }
}
