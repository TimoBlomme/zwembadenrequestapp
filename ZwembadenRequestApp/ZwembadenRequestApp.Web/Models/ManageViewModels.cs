using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace ZwembadenRequestApp.Web.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    public class QuoteRequestViewModel
    {
        [Required]
        [Display(Name = "Type Zwembad")]
        public string PoolType { get; set; }

        [Required]
        [Display(Name = "Lengte (m)")]
        [Range(1, 50, ErrorMessage = "Lengte moet tussen 1 en 50 meter zijn")]
        public decimal Length { get; set; }

        [Required]
        [Display(Name = "Breedte (m)")]
        [Range(1, 50, ErrorMessage = "Breedte moet tussen 1 en 50 meter zijn")]
        public decimal Width { get; set; }

        [Required]
        [Display(Name = "Diepte (m)")]
        [Range(0.5, 5, ErrorMessage = "Diepte moet tussen 0.5 en 5 meter zijn")]
        public decimal Depth { get; set; }

        [Display(Name = "Aantal spots")]
        [Range(0, 10, ErrorMessage = "Aantal spots moet tussen 0 en 10 zijn")]
        public int NumberOfLights { get; set; }

        [Display(Name = "Heeft trap")]
        public bool HasStairs { get; set; }

        [Display(Name = "Extra opmerkingen")]
        public string AdditionalNotes { get; set; }
    }

    public class AdminQuoteViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Klant Naam")]
        public string CustomerName { get; set; }

        [Display(Name = "Configuratie")]
        public string Configuration { get; set; }

        [Required(ErrorMessage = "Status is verplicht")]
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Voorgestelde Prijs")]
        [Range(0, 1000000, ErrorMessage = "Prijs moet tussen 0 en 1.000.000 zijn")]
        [DataType(DataType.Currency)]
        public decimal? ProposedPrice { get; set; }

        [Display(Name = "Admin Opmerkingen")]
        [StringLength(1000, ErrorMessage = "Opmerkingen mogen maximaal 1000 karakters zijn")]
        public string AdminNotes { get; set; }

        [Display(Name = "Aanvraag Datum")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime RequestDate { get; set; }

        [Display(Name = "Reactie Datum")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? ResponseDate { get; set; }

        // Original quote details for reference
        [Display(Name = "Type Zwembad")]
        public string PoolType { get; set; }

        [Display(Name = "Lengte (m)")]
        public decimal Length { get; set; }

        [Display(Name = "Breedte (m)")]
        public decimal Width { get; set; }

        [Display(Name = "Diepte (m)")]
        public decimal Depth { get; set; }

        [Display(Name = "Aantal Spots")]
        public int NumberOfLights { get; set; }

        [Display(Name = "Heeft Trap")]
        public bool HasStairs { get; set; }

        [Display(Name = "Klant Opmerkingen")]
        public string AdditionalNotes { get; set; }
    }
}