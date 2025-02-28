using GourmetShop.DataAccess.Data;
using GourmetShop.DataAccess.Repositories;
using GourmetShop.DataAccess.Repositories.Interfaces.CRUD_Subinterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GourmetShop.DataAccess.Models;

var builder = WebApplication.CreateBuilder(args);

// Reads from the text file that contains the connection string
string connectionString = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "net9.0", "ConnectionString.txt"));
// So JSON needs \\ to read one \, but the connection string needs \\ to read one \.
builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"] = connectionString.Replace(@"\", @"\\");

// Add services to the container.
// ASSUMPTION: The database hasn't been created already
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GourmetShopDbContext>(options =>
    options
    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("GourmetShop.DataAccess"))
    .UseSeeding((context, _) =>
         {
             DataInitializer dataInitializer = new DataInitializer();
             dataInitializer.Initialize((GourmetShopDbContext)context);
         })
    );

builder.Services.AddDefaultIdentity<Authentication>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<GourmetShopDbContext>();

// TODO: Add the necessary controllers and DI here
//?? We have a user repository and an authentication repository. Wouldn't that be the same controller
//that would redirect them to their respective pages?
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserInfoRepository, UserInfoRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// For development purposes, we will use the following code seed the database
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
    pattern: "{controller=Home}/{action=Index}/{id?}")
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
