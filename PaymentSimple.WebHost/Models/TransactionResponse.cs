using PaymentSimple.Core.Domain.Models;
using PaymentSimple.WebHost.Extensions;

namespace PaymentSimple.WebHost.Models
{
    public class TransactionResponse
    {
        public TransactionResponse()
        {

        }

        public TransactionResponse(Payment payment)
        {
            Id = payment.Id;
            Status = payment.Status.GetStatusValue();
        }

        public Guid Id { get; set; }// payment id

        public Status Status { get; set; }


    }
}
