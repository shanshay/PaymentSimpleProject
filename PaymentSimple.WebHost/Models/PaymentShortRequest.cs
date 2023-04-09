namespace PaymentSimple.WebHost.Models
{
    public class PaymentShortRequest
    {
        public Guid Id { get; set; }

        public Guid OrderReference { get; set; }
    }
}
