namespace PaymentSimple.Exceptions
{
    public class IncorrectStatusValueException : Exception
    {
        public IncorrectStatusValueException(int value)
            : base($"Incorrect value '{value}' of Status")
        {

        }
    }
}
