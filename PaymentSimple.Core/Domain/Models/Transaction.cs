using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentSimple.Core.Domain.Models
{
    public class Transaction : BaseEntity
    {
        public Transaction(Payment payment)
        {
            Id = Guid.NewGuid();
            PaymentId = payment.Id;
            OrderId = payment.OrderId;
            DateTime = DateTime.Now;
            Status = payment.Status;
        }

        [ForeignKey("Payment")]
        public Guid PaymentId { get; set; }

        public virtual Payment Payment {get;set;}

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }

        public DateTime DateTime { get; set; } 
        
        public int Status { get; set; }
    }
}
