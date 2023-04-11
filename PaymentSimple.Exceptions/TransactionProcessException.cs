namespace PaymentSimple.Exceptions
{
    public class TransactionProcessException : Exception
    {
        public TransactionProcessException()
            : base($"Transaction end with critical error")
        {

        }
    }
}
