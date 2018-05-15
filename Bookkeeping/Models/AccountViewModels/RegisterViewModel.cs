using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookkeeping.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Heslo musí mít minimálně 6 znaků, maximálně 100", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Heslo")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Ověřit heslo")]
        [Compare("Password", ErrorMessage = "Hesla se neshodují")]
        public string ConfirmPassword { get; set; }
    }
}
