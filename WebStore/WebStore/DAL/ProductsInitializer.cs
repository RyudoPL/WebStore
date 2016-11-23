using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebStore.Models;
using WebStore.Migrations;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebStore.DAL
{
    public class ProductsInitializer : MigrateDatabaseToLatestVersion<ProductsContext, Configuration>
    {
        public static void SeedProductsData(ProductsContext context)
        {
            //List of all our categories
            var categories = new List<Category>
            {
                new Category() {CategoryId = 1, CategoryName = "Books", IconFileName = "Books.png", CategoryDescription = "Books description"},
                new Category() {CategoryId = 2, CategoryName = "Cars", IconFileName = "Cars.png", CategoryDescription = "Cars description"},
                new Category() {CategoryId = 3, CategoryName = "Mobile phones", IconFileName = "MobilePhones.png", CategoryDescription = "Mobile phones description"},
                new Category() {CategoryId = 4, CategoryName = "Furniture", IconFileName = "Furniture.png", CategoryDescription = "Furniture description"},
                new Category() {CategoryId = 5, CategoryName = "Games", IconFileName = "Games.png", CategoryDescription = "Games description"}
            };

            categories.ForEach(category => context.Category.AddOrUpdate(category));
            context.SaveChanges();

            //List of all our products
            var products = new List<Product>
             {
                 new Product() {ProductId = 1, CategoryId = 1, ProductName = "Lorem ipsum", ProductMerchant = "Lorem ipsum", ProductDescription = "Lorem ipsum", ProductPrice = 20, DateAdded = DateTime.Today, ImageFileName = "dell.jpg", Bestseller = true, Hidden = false },
                 new Product() {ProductId = 2, CategoryId = 1, ProductName = "Lorem ipsum", ProductMerchant = "Lorem ipsum", ProductDescription = "Lorem ipsum", ProductPrice = 20, DateAdded = DateTime.Today, ImageFileName = "dell.jpg",  Bestseller = true, Hidden = false },
                 new Product() {ProductId = 3, CategoryId = 1, ProductName = "Lorem ipsum", ProductMerchant = "Lorem ipsum", ProductDescription = "Lorem ipsum", ProductPrice = 20, DateAdded = DateTime.Today, ImageFileName = "dell.jpg",  Bestseller = true, Hidden = false },
                 new Product() {ProductId = 4, CategoryId = 1, ProductName = "Lorem ipsum", ProductMerchant = "Lorem ipsum", ProductDescription = "Lorem ipsum", ProductPrice = 20, DateAdded = DateTime.Today, ImageFileName = "dell.jpg",  Bestseller = true, Hidden = false },
                 new Product() {ProductId = 5, CategoryId = 1, ProductName = "Lorem ipsum", ProductMerchant = "Lorem ipsum", ProductDescription = "Lorem ipsum", ProductPrice = 20, DateAdded = DateTime.Today, ImageFileName = "dell.jpg",  Bestseller = true, Hidden = false },
                 new Product() {ProductId = 6, CategoryId = 1, ProductName = "Harry Potter", ProductMerchant = "Wydawnictwo Egmont", ProductDescription = "rg", ProductPrice = 20, DateAdded = DateTime.Today, ImageFileName = "dell.jpg",  Bestseller = true, Hidden = false }
             };

            products.ForEach(product => context.Product.AddOrUpdate(product));
            context.SaveChanges();
        }

        /// <summary>
        /// Seeds special users to our database and assigns admin roles to them
        /// </summary>
        /// <param name="db">Reference to our context</param>
        public static void SeedUsers(ProductsContext db)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            const string name = "admin@webstore.pl";
            const string password = "Password";
            const string roleName = "Admin";

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name, UserData = new UserData() };
                var result = userManager.Create(user, password);
            }

            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var roleResult = roleManager.Create(role);
            }

            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
    }
}
