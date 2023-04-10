namespace PaymentSimple.Exceptions
{
    public class IncorrectPageValueException : Exception
    {
        public IncorrectPageValueException(int page)
            : base($"Incorrect page value '{page}'. Page should be more than 0.")
        {

        }
    }
}
