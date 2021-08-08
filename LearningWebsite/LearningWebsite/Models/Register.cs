using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LearningWebsite.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Please enter your Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm Password")]
        [Compare("Password", ErrorMessage = "Both Password and Confirm Password Must be Same")]
        public string ConfirmPassword { get; set; }
    }
}