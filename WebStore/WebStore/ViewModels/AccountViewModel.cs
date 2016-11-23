using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebStore.ViewModels
{
    /// <summary>
    /// Model that stores properties used when client logs in
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "You need to enter valid e-mail address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "You need to enter valid password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// Model that stores properties used when client registers
    /// </summary>
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20,ErrorMessage = "{0} need to be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "It doesn't match your password.")]
        public string ConfirmPassword { get; set; }
    }
}