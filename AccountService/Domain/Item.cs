using System.ComponentModel.DataAnnotations;

namespace AccountService.Domain
{
    public class Item
    {
        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string Title { get; set; }

        public decimal Amount { get; set; }

        [Required]
        public Currency Currency { get; set; }

        public string AccountName { get; set; }

        [Required]
        public TimePeriod Period { get; set; }

        [Required]
        public string Icon { get; set; }
    }
}
