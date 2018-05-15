using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    public class InvoiceItem
    {
        public int? InvoiceItemId { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [JsonIgnore]
        public Invoice Invoice { get; set; }

        [Required(ErrorMessage = "Zadejte počet")]
        [Display(Name = "Počet")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Zadejte měrnou jednotku")]
        [Display(Name = "MJ")]
        public string Unit { get; set; }

        [Required(ErrorMessage = "Zadejte popis položky")]
        [Display(Name = "Popis")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Zadejte cenu za měrnou jednotku položky")]
        [Display(Name = "Cena za MJ")]
        public int Price { get; set; }
    }
}
