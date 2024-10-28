namespace AccountService.Domain
{
    public class Saving
    {
        public decimal Amount { get; set; }

        public Currency Currency { get; set; }

        public decimal Interest { get; set; }

        public bool Deposit { get; set; }

        public bool Capitalization { get; set; }
    }
}