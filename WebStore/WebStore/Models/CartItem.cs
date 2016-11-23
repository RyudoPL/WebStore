using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebStore.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Amount { get; set; }
        public decimal Value { get; set; }
    }
}