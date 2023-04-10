using System.Security.Policy;

namespace PaymentSimple.Exceptions
{
    public class CardExpiredException : Exception
    {
        public CardExpiredException(string number)
            : base($"Card with number {number} has expired")
        {

        }
    }
}
