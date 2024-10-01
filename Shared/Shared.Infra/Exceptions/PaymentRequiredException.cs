using Shared.Infra.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infra.Exceptions
{
    public class PaymentRequiredException : ExceptionBase
    {
        public PaymentRequiredException(string message) : base(message)
        {
        }

        public PaymentRequiredException(string message, List<ModelStateError> modelState) : base(message, modelState)
        {
        }

        public PaymentRequiredException(string message, string stringError) : base(message, stringError)
        {
        }
    }
}
