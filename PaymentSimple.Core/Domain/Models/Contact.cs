namespace PaymentSimple.Core.Domain.Models
{
    public class Contact : BaseEntity
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public virtual List<Card> Cards { get; set; }
    }
}
