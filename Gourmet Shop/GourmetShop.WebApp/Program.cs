using GourmetShop.DataAccess.Data;
using GourmetShop.DataAccess.Repositories;
using GourmetShop.DataAccess.Repositories.Interfaces.CRUD_Subinterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GourmetShop.DataAccess.Models;
using System.Reflection.Metadata;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

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

builder.Services.AddDefaultIdentity<Authentication>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<GourmetShopDbContext>();

// TODO: Add the necessary controllers and DI here
//?? We have a user repository and an authentication repository. Wouldn't that be the same controller
//that would redirect them to their respective pages?
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserInfoRepository, UserInfoRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
