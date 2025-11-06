using System;
using System.ComponentModel.DataAnnotations;

namespace ZwembadenRequestApp.Core.Entities
{
    public class QuoteRequest
    {
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string PoolType { get; set; }

        [Required]
        public decimal Length { get; set; }

        [Required]
        public decimal Width { get; set; }

        [Required]
        public decimal Depth { get; set; }

        public int NumberOfLights { get; set; }
        public bool HasStairs { get; set; }
        public string AdditionalNotes { get; set; }

        public decimal? ProposedPrice { get; set; }
        public string Status { get; set; } = "New";
        public string AdminNotes { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;
        public DateTime? ResponseDate { get; set; }

        // Computed property for display
        public string Configuration =>
            $"Type: {PoolType}, Afmetingen: {Length}m x {Width}m x {Depth}m, " +
            $"Spots: {NumberOfLights}, Trap: {(HasStairs ? "Ja" : "Nee")}" +
            $"{(string.IsNullOrEmpty(AdditionalNotes) ? "" : $", Opmerkingen: {AdditionalNotes}")}";
    }

    public static class QuoteStatus
    {
        public const string New = "New";
        public const string InProgress = "In Progress";
        public const string Done = "Done";
    }
}