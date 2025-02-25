using GourmetShop.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GourmetShop.DataAccess.Data
{
    public class DataInitializer
    {
        public void Initialize(GourmetShopDbContext context)
        {
            try
            {
                if (!context.Authentications.Any())
                {
                    context.Authentications.AddRange(SeedAuthenticationData());
                    context.SaveChanges();
                }

                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(SeedIdentityRoleData());
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(SeedUserData());
                    context.SaveChanges();
                }

                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(SeedCategoryData());
                    context.SaveChanges();
                }

                if (!context.Subcategories.Any())
                {
                    context.Subcategories.AddRange(SeedSubcategoryData());
                    context.SaveChanges();
                }

                if (!context.Suppliers.Any())
                {
                    context.Suppliers.AddRange(SeedSupplierData());
                    context.SaveChanges();
                }

                if (!context.Products.Any())
                {
                    context.Products.AddRange(SeedProductData());
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // TODO: Add phone numbers to the JSON file
        private List<Authentication> SeedAuthenticationData()
        {
            List<Authentication> authentications = new List<Authentication>();

            // The reason why we need this is because we're copying the JSON file to the output directory
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(assemblyFolder, "Authentication.json");

            // Modify this to grab the SeedData from the JSON file, it's in a separate project
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                Dictionary<string, List<Authentication>> data = JsonConvert.DeserializeObject<Dictionary<string, List<Authentication>>>(json);

                authentications = data["Authentication"];
            }

            return authentications;
        }

        public List<IdentityRole> SeedIdentityRoleData()
        {
            var roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            };
            return roles;
        }

        public List<Category> SeedCategoryData()
        {
           List<Category> categories = new List<Category>();

            // The reason why we need this is because we're copying the JSON file to the output directory
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(assemblyFolder, "Categories.json");

            // Modify this to grab the SeedData from the JSON file, it's in a separate project
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                Dictionary<string, List<Category>> data = JsonConvert.DeserializeObject<Dictionary<string, List<Category>>>(json);
            
                categories = data["Categories"];
            }

            return categories.Select(c => new Category
            {
                Name = c.Name
            }).ToList();
        }

        public List<Subcategory> SeedSubcategoryData()
        {
            List<Subcategory> subcategories = new List<Subcategory>();

            // The reason why we need this is because we're copying the JSON file to the output directory
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(assemblyFolder, "Subcategories.json");

            // Modify this to grab the SeedData from the JSON file, it's in a separate project
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                Dictionary<string, List<Subcategory>> data = JsonConvert.DeserializeObject<Dictionary<string, List<Subcategory>>>(json);

                subcategories = data["Subcategories"];
            }

            // Prevents the IDENTITY_INSERT issue
            return subcategories.Select(s => new Subcategory
            {
                Name = s.Name,
                CategoryId = s.CategoryId
            }).ToList();
        }

        public List<Product> SeedProductData()
        {
            List<Product> products = new List<Product>();

            // The reason why we need this is because we're copying the JSON file to the output directory
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(assemblyFolder, "Products.json");

            // Modify this to grab the SeedData from the JSON file, it's in a separate project
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                Dictionary<string, List<Product>> data = JsonConvert.DeserializeObject<Dictionary<string, List<Product>>>(json);
                products = data["Products"];
            }

            return products.Select(p => new Product
            {
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                SubcategoryId = p.SubcategoryId,
                Package = p.Package,
                IsDiscontinued = p.IsDiscontinued,
                SupplierId = p.SupplierId
            }).ToList();
        }

        public List<Supplier> SeedSupplierData()
        {
            List<Supplier> suppliers = new List<Supplier>();

            // The reason why we need this is because we're copying the JSON file to the output directory
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(assemblyFolder, "Suppliers.json");

            // Modify this to grab the SeedData from the JSON file, it's in a separate project
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                Dictionary<string, List<Supplier>> data = JsonConvert.DeserializeObject<Dictionary<string, List<Supplier>>>(json);
                suppliers = data["Suppliers"];
            }

            // Prevents the IDENTITY_INSERT issue
            return suppliers.Select(s => new Supplier
            {
                CompanyName = s.CompanyName,
                ContactName = s.ContactName,
                ContactTitle = s.ContactTitle,
                City = s.City,
                Country = s.Country,
                Phone = s.Phone,
                Fax = s.Fax
            }).ToList();
        }

        public List<UserInfo> SeedUserData()
        {
            List<UserInfo> users = new List<UserInfo>();

            // The reason why we need this is because we're copying the JSON file to the output directory
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(assemblyFolder, "Users.json");

            // Modify this to grab the SeedData from the JSON file, it's in a separate project
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                Dictionary<string, List<UserInfo>> data = JsonConvert.DeserializeObject<Dictionary<string, List<UserInfo>>>(json);

                users = data["Users"];
            }

            return users.Select(u => new UserInfo
            {
                AuthenticationId = u.AuthenticationId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                City = u.City,
                Country = u.Country
            }).ToList();
        }
    }
}
