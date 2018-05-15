using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    public enum ContactType
    {
        Subscriber,
        Supplier,
        Both
    }

    public class Contact
    {
        public static List<SelectListItem> TypeItems = new List<SelectListItem>
        {
            new SelectListItem {Text = "Odběratel", Value = ContactType.Subscriber.ToString()},
            new SelectListItem {Text = "Dodavatel", Value = ContactType.Supplier.ToString()},
            new SelectListItem {Text = "Dodavatel i odběratel", Value = ContactType.Both.ToString()}
        };
        public int ContactId { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "Uživatel")]
        public string ApplicationUserId { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "Uživatel")]
        public ApplicationUser ApplicationUser { get; set; }

        [Required(ErrorMessage = "Zadejte název / jméno a příjmení")]
        [Display(Name = "Název / Jméno a příjmení")]
        public string Name { get; set; }

        [Display(Name = "Adresa")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Zadejte zda-li se jedná o odběratele nebo dodavatele")]
        [Display(Name = "Typ kontaktu")]
        public ContactType Type { get; set; }

        [Display(Name = "IČO")]
        public string ICO { get; set; }

        [Display(Name = "DIČ")]
        public string DIC { get; set; }
    }
}
