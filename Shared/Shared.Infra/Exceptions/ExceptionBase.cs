using Shared.Infra.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infra.Exceptions
{
    public class ExceptionBase : Exception
    {
        public List<ModelStateError> ModelState { get; }
        public string ErrorString { get; }

        public ExceptionBase(string message) : base(message)
        {
            ModelState = new();
        }

        public ExceptionBase(string message, List<ModelStateError> modelState) : base(message)
        {
            ModelState = modelState;
        }

        public ExceptionBase(string message, string stringError) : base(message)
        {
            ErrorString = stringError;
            ModelState = new();
        }

    }
}
