using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentSimple.Exceptions
{
    public class PaymentIdIsNullException : Exception
    {
        public PaymentIdIsNullException()
            : base($"Payment Id in request is null.")
        {

        }
    }
}
