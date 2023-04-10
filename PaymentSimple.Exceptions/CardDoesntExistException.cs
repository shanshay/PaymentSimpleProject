namespace PaymentSimple.Exceptions
{
    public class CardDoesntExistException : Exception
    {
        public CardDoesntExistException(string number)
            : base($"Card with number {number} doesn't exist in system")
        {

        }
    }
}
