using Shared.Infra.DTO;

namespace Shared.Infra.Exceptions
{
    public class NotFoundException : ExceptionBase
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, List<ModelStateError> modelState) : base(message, modelState)
        {
        }

        public NotFoundException(string message, string stringError) : base(message, stringError)
        {
        }
    }
}
