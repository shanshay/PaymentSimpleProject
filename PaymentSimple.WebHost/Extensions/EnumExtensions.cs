using PaymentSimple.Exceptions;
using PaymentSimple.WebHost.Models;

namespace PaymentSimple.WebHost.Extensions
{
    public static class EnumExtensions
    {
        public static Status GetStatusValue(this int value)
        {
            try
            {
                return (Status)Enum.GetValues(typeof(Status)).GetValue(value);
            }
            catch
            {
                throw new IncorrectStatusValueException(value);
            }
        }
    }
}
