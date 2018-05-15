using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    public class UserContact
    {
        public int UserContactId { get; set; }

        [Required(ErrorMessage = "Zadejte název / jméno a příjmení")]
        [Display(Name = "Název / Jméno a příjmení")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Zadejte adresu")]
        [Display(Name = "Adresa")]
        public string Address { get; set; }

        [Display(Name = "IČO")]
        public int ICO { get; set; }

        [Display(Name = "DIČ")]
        public string DIC { get; set; }

        [Required]
        [Display(Name = "Plátce DPH")]
        public bool DPHPayer { get; set; }
    }
}
