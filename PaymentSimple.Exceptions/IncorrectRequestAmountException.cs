namespace PaymentSimple.Exceptions
{
    public class IncorrectRequestAmountException : Exception
    {
        public IncorrectRequestAmountException(decimal amount)
            : base($"Incorrect request amount '{amount}'. Amount should be more than 0.")
        {

        }
    }
}
