using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentSimple.Core.Domain.Models
{
    public class Card : BaseEntity
    {
        public string CardNumber { get; set; }

        [ForeignKey("Contact ")]
        public Guid ContactId { get; set; }

        public virtual Contact Contact { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }

        public int CVV { get; set; }

        public virtual List<Order> Orders { get; set; } = new List<Order>();

        public virtual List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
