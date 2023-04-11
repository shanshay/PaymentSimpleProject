namespace PaymentSimple.WebHost.Models
{
    public class TransactionShortRequest
    {
        public Guid Id { get; set; }

        public Guid OrderReference { get; set; }
    }
}
