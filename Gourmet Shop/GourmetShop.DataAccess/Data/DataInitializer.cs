using GourmetShop.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
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
        // TODO: Call this somewhere, maybe in the Program.cs file, need to have seed data for Authentication and fix the Users.json file to have the correct AuthenticationID
        public void Initialize(GourmetShopDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(SeedIdentityRoleData());
                context.SaveChanges();
            }

            if (!context.Authentications.Any())
            {
                context.Authentications.AddRange(SeedAuthenticationData());
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(SeedUserData());
                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                context.Products.AddRange(SeedProductData());
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
        }

        // TODO: Add phone numbers to the JSON file
        private List<Authentication> SeedAuthenticationData()
        {
            var authentications = new List<Authentication>();

            // The reason why we need this is because we're copying the JSON file to the output directory
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(assemblyFolder, "Authentication.json");

            // Modify this to grab the SeedData from the JSON file, it's in a separate project
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                authentications = JsonConvert.DeserializeObject<List<Authentication>>(json);
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
            var categories = new List<Category>();

            // The reason why we need this is because we're copying the JSON file to the output directory
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(assemblyFolder, "Categories.json");

            // Modify this to grab the SeedData from the JSON file, it's in a separate project
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                categories = JsonConvert.DeserializeObject<List<Category>>(json);
            }

            return categories;
        }

        public List<Subcategory> SeedSubcategoryData()
        {
            var subcategories = new List<Subcategory>();

            // The reason why we need this is because we're copying the JSON file to the output directory
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(assemblyFolder, "Subcategories.json");

            // Modify this to grab the SeedData from the JSON file, it's in a separate project
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                subcategories = JsonConvert.DeserializeObject<List<Subcategory>>(json);
            }

            return subcategories;
        }

        public List<Product> SeedProductData()
        {
            var products = new List<Product>();

            // The reason why we need this is because we're copying the JSON file to the output directory
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(assemblyFolder, "Products.json");

            // Modify this to grab the SeedData from the JSON file, it's in a separate project
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                products = JsonConvert.DeserializeObject<List<Product>>(json);
            }

            return products;
        }

        public List<Supplier> SeedSupplierData()
        {
            var suppliers = new List<Supplier>();

            // The reason why we need this is because we're copying the JSON file to the output directory
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(assemblyFolder, "Suppliers.json");

            // Modify this to grab the SeedData from the JSON file, it's in a separate project
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                suppliers = JsonConvert.DeserializeObject<List<Supplier>>(json);
            }

            return suppliers;
        }

        public List<UserInfo> SeedUserData()
        {
            var users = new List<UserInfo>();

            // The reason why we need this is because we're copying the JSON file to the output directory
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string jsonFilePath = Path.Combine(assemblyFolder, "Users.json");

            // Modify this to grab the SeedData from the JSON file, it's in a separate project
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                users = JsonConvert.DeserializeObject<List<UserInfo>>(json);
            }

            return users;
        }
    }
}
