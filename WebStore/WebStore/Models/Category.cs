using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Enter category's name")]
        [StringLength(50)]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Enter category's description")]
        public string CategoryDescription { get; set; }
        public string IconFileName { get; set; }
       
        //List of products
        public virtual ICollection<Product> Products { get; set; }
    }
}