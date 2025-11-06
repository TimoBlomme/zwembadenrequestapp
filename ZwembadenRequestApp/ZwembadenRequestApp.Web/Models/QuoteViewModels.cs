using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ZwembadenRequestApp.Web.Models
{
    public class CreateQuoteViewModel
    {
        [Required(ErrorMessage = "Pool type is verplicht")]
        [Display(Name = "Type Zwembad")]
        public string PoolType { get; set; }

        [Required(ErrorMessage = "Lengte is verplicht")]
        [Range(1, 50, ErrorMessage = "Lengte moet tussen 1 en 50 meter zijn")]
        [Display(Name = "Lengte (m)")]
        public decimal Length { get; set; }

        [Required(ErrorMessage = "Breedte is verplicht")]
        [Range(1, 50, ErrorMessage = "Breedte moet tussen 1 en 50 meter zijn")]
        [Display(Name = "Breedte (m)")]
        public decimal Width { get; set; }

        [Required(ErrorMessage = "Diepte is verplicht")]
        [Range(0.5, 5, ErrorMessage = "Diepte moet tussen 0.5 en 5 meter zijn")]
        [Display(Name = "Diepte (m)")]
        public decimal Depth { get; set; }

        [Required(ErrorMessage = "Aantal spots is verplicht")]
        [Range(0, 10, ErrorMessage = "Aantal spots moet tussen 0 en 10 zijn")]
        [Display(Name = "Aantal Spots")]
        public int NumberOfLights { get; set; }

        [Display(Name = "Heeft Trap")]
        public bool HasStairs { get; set; }

        [Display(Name = "Extra Opmerkingen")]
        [StringLength(500, ErrorMessage = "Opmerkingen mogen maximaal 500 karakters zijn")]
        public string AdditionalNotes { get; set; }
    }

    public class QuoteDetailsViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string PoolType { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Depth { get; set; }
        public int NumberOfLights { get; set; }
        public bool HasStairs { get; set; }
        public string AdditionalNotes { get; set; }
        public decimal? ProposedPrice { get; set; }
        public string Status { get; set; }
        public string AdminNotes { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string Configuration { get; set; }
    }
}