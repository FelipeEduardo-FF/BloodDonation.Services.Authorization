namespace Shared.Infra.Results.Errors
{
    public partial class OperationResult
    {

        public static Result<TValue> NotFound<TValue>(string message) =>
            Result<TValue>.Fail(new Error(404, message, ErrorType.NotFound));

        public static Result<TValue> BadRequest<TValue>(string message) =>
            Result<TValue>.Fail(new Error(400, message, ErrorType.Validation));

        public static Result<TValue> Unauthorized<TValue>(string message) =>
            Result<TValue>.Fail(new Error(401, message, ErrorType.Unauthorized));

        public static Result<TValue> Forbidden<TValue>(string message) =>
            Result<TValue>.Fail(new Error(403, message, ErrorType.Forbidden));

        public static Result<TValue> InternalServerError<TValue>(string message) =>
            Result<TValue>.Fail(new Error(500, message, ErrorType.ServerError));

        public static Result<TValue> Fail<TValue>(IEnumerable<IError> errors) =>
            Result<TValue>.Fail(errors);

        public static Result<TValue> Ok<TValue>(TValue value) => 
            Result<TValue>.Ok(value);

    }
}
