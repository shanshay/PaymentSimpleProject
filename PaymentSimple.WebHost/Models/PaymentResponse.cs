namespace PaymentSimple.WebHost.Models
{
    public class PaymentResponse
    {
        public Guid Id { get; set; }// payment id

        public Status Status { get; set; } //Authorized
    }
}
