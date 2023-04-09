using PaymentSimple.Exceptions;
using PaymentSimple.WebHost.Models;
using System.Text;

namespace PaymentSimple.WebHost.Extensions
{
    public static class CardNumberExtensions
    {
        static int firstSymbols = 6;
        static int lastSymbols = 4;
        public static string HideCardNumber(this string number)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{number.Substring(0, firstSymbols)}");
            stringBuilder.Append($"{new string('*', number.Length - firstSymbols - lastSymbols)}");
            stringBuilder.Append(number.Substring(number.Length - lastSymbols));

            return stringBuilder.ToString();
        }
    }
}
