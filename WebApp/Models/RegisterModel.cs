using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class RegisterModel
    {

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }        

        [Required]
        [Display(Name = "Nick Name")]
        public string NickName { get; set; }

    }
}
