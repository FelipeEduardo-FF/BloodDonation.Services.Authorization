using Shared.Infra.DTO;

namespace Shared.Infra.Exceptions
{
    public class ForbiddenExcept : ExceptionBase
    {
        public ForbiddenExcept(string message) : base(message)
        {
        }

        public ForbiddenExcept(string message, List<ModelStateError> modelState) : base(message, modelState)
        {
        }

        public ForbiddenExcept(string message, string stringError) : base(message, stringError)
        {
        }
    }
}
