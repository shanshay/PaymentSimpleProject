using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentSimple.Core.Domain.Models
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }

        [ForeignKey("Card ")]
        public Guid CardId { get; set; }

        public virtual Card Card { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public virtual List<Payment> Payments { get; set; } = new List<Payment>(); 

        public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
