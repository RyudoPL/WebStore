using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebStore.Models;

namespace WebStore.DAL
{
    public class ProductsContext : IdentityDbContext<ApplicationUser>
    {
        public ProductsContext() : base("ProductsContext")
        {
        }
        static ProductsContext()
        {
            Database.SetInitializer(new ProductsInitializer());
        }

        public static ProductsContext Create()
        {
            return new ProductsContext();
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
    }
}