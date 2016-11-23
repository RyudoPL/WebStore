using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebStore.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Enter product's name")]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Enter product's merchant")]
        [StringLength(50)]
        public string ProductMerchant { get; set; }

        public DateTime DateAdded { get; set; }

        [StringLength(100)]
        public string ImageFileName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public bool Bestseller { get; set; }
        public bool Hidden { get; set; }

        public string ShortDescription { get; set; }

        public virtual Category Category { get; set; }
    }
}