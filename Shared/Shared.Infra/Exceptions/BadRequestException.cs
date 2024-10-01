using Shared.Infra.DTO;

namespace Shared.Infra.Exceptions
{
    public class BadRequestException : ExceptionBase
    {
        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, List<ModelStateError> modelState) : base(message, modelState)
        {
        }

        public BadRequestException(string message, string stringError) : base(message, stringError)
        {
        }
    }
}
