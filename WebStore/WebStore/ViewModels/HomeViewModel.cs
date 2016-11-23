using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebStore.Models;

namespace WebStore.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> NewProducts { get; set; }
        public IEnumerable<Product> Bestsellers { get; set; }
    }
}