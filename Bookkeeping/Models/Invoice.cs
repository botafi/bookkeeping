using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    public enum InvoiceType
    {
        Income,
        Expense
    }
    public class Invoice
    {
        public static List<SelectListItem> InvoiceTypeItems = new List<SelectListItem>
        {
            new SelectListItem {Text = "Příjem", Value = InvoiceType.Income.ToString()},
            new SelectListItem {Text = "Náklad", Value = InvoiceType.Expense.ToString()},
        };
        public int InvoiceId { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "Uživatel")]
        public string ApplicationUserId { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        [Display(Name = "Uživatel")]
        public ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Položky")]
        public IEnumerable<InvoiceItem> InvoiceItems { get; set; }

        [Required(ErrorMessage = "Vyberte odběratele/dodavatele")]
        [Display(Name = "Odběratel/Dodavatel")]
        public int ContactId { get; set; }

        //[Required(ErrorMessage = "Vyberte odběratele/dodavatele")]
        [Display(Name = "Odběratel/Dodavatel")]
        public Contact Contact { get; set; }

        [Display(Name = "Datum splatnosti")]
        [Required(ErrorMessage = "Vyberte datum splatnosti")]
        [DataType(DataType.Date)]
        public DateTime DueTime { get; set; }

        [Required(ErrorMessage = "Vyberte datum vystavení")]
        [Display(Name = "Datum vystavení")]
        [DataType(DataType.Date)]
        public DateTime DateOfIssue { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Zadejte číslo faktury")]
        [Display(Name = "Číslo faktury")]
        public string Title { get; set; }

        [Display(Name = "Typ")]
        [Required(ErrorMessage = "Zvolte zda-li se jedná o příjem nebo náklad")]
        public InvoiceType InvoiceType { get; set; }

    }
}
