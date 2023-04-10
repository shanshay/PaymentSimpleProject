namespace PaymentSimple.Exceptions
{
    public class PaymentDoesntExistException : Exception
    {
        public PaymentDoesntExistException(string id)
            : base($"Payment with id {id} doesn't exist")
        {

        }
    }
}
