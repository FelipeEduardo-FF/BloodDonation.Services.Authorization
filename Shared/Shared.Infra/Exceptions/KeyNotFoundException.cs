using Shared.Infra.DTO;

namespace Shared.Infra.Exceptions
{
    public class KeyNotFoundException : ExceptionBase
    {
        public KeyNotFoundException(string message) : base(message)
        {
        }

        public KeyNotFoundException(string message, List<ModelStateError> modelState) : base(message, modelState)
        {
        }

        public KeyNotFoundException(string message, string stringError) : base(message, stringError)
        {
        }
    }
}
