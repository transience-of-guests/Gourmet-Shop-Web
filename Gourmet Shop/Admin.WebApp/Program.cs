using GourmetShop.DataAccess.Data;
using GourmetShop.DataAccess.Models;
using GourmetShop.DataAccess.Repositories;
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
            }).AddEntityFrameworkStores<GourmetShopDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI()
            .AddRoles<IdentityRole>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
            builder.Services.AddScoped<IUserInfoRepository, UserInfoRepository>();

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

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<Authentication>>();

                Task.Run(async () =>
                {
                    try
                    {

                        var adminRoleExists = await roleManager.RoleExistsAsync("Admin");
                        if (!adminRoleExists)
                        {
                            await roleManager.CreateAsync(new IdentityRole("Admin"));
                        }


                        string adminEmail = "admin@admin.com";
                        var adminUser = await userManager.FindByEmailAsync(adminEmail);

                        if (adminUser == null)
                        {
                            var newAdmin = new Authentication
                            {
                                UserName = adminEmail,
                                Email = adminEmail,
                                EmailConfirmed = true,
                                UserInfo = new UserInfo
                                {
                                    FirstName = "Admin",
                                    LastName = "User",
                                    AuthenticationId = adminEmail,
                                    City = "AdminCity",
                                    Country = "AdminCountry"
                                }
                            };

                            var createAdminResult = await userManager.CreateAsync(newAdmin, "Admin@123");

                            if (createAdminResult.Succeeded)
                            {
                                await userManager.AddToRoleAsync(newAdmin, "Admin");

                            }
                            else
                            {
                                Console.WriteLine("Failure.");
                                foreach (var error in createAdminResult.Errors)
                                {
                                    Console.WriteLine($" {error.Description}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Admin user already exists.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error seeding admin user: {ex.Message}");
                    }
                }).GetAwaiter().GetResult();
            }

            app.Run();
        }
    }
}
