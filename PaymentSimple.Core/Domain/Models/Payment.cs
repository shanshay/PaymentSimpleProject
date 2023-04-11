using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentSimple.Core.Domain.Models
{
    public class Payment : BaseEntity
    {
        public decimal PaymentAmount { get; set; }

        public string PaymentCurrency { get; set; }

        [ForeignKey("Card ")]
        public Guid CardId { get; set; }

        public virtual Card Card { get; set; }

        [ForeignKey("Order ")]
        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int Status { get; set; }

        public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
