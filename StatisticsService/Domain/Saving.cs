using System.ComponentModel.DataAnnotations;

namespace StatisticsService.Domain
{
    public class Saving
    {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public Currency Currency { get; set; }

        [Required]
        public decimal Interest { get; set; }

        [Required]
        public bool Deposit { get; set; }

        [Required]
        public bool Capitalization { get; set; }
    }
}