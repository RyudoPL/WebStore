using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser user { get; set; }

        [Required(ErrorMessage = "Enter your name")]
        [StringLength(50)]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Enter your surname")]
        [StringLength(50)]
        public string CustomerSurname { get; set; }
        [Required(ErrorMessage = "Enter your address")]
        [StringLength(100)]
        public string Adress { get; set; }
        [Required(ErrorMessage = "Enter your city")]
        [StringLength(100)]
        public string City { get; set; }
        [Required(ErrorMessage = "Enter your zipcode")]
        [StringLength(6)]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Enter your phone number")]
        [StringLength(20)]
        [RegularExpression(@"(\+\d{2})*[\d\s -]+", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Enter your e-mail address")]
        [EmailAddress(ErrorMessage = "Invalid e-mail address")]
        public string Email { get; set; }
        public string Comment { get; set; }
        public DateTime DateAdded { get; set; }
        public decimal OrderValue { get; set; }
        public OrderState OrderState { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }

    public enum OrderState
    {
        New,
        Completed
    }
}