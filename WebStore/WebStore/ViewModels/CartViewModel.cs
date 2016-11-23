using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebStore.Models;

namespace WebStore.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal TotalValue { get; set; }
    }
}