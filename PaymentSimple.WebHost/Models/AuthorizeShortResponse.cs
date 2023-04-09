namespace PaymentSimple.WebHost.Models
{
    public class AuthorizeShortResponse
    {
        public decimal Amount { get; set; } //payment amount

        public string Currency { get; set; } //string ISO3 payment currency

        public string HolderName { get; set; } //credit card number first 6 digit and last 4 digit

        public Guid Id { get; set; } // payment Id

        public Status Status { get; set; } //status Captured
    }
}
