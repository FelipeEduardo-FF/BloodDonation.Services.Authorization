namespace Shared.Infra.Results.Errors
{
    public static partial class OperationResult
    {
        public static Result NotFound(string message) =>
            Result.Fail(new Error(404, message, ErrorType.NotFound));

        public static Result BadRequest(string message) =>
            Result.Fail(new Error(400, message, ErrorType.Validation));

        public static Result Unauthorized(string message) =>
            Result.Fail(new Error(401, message, ErrorType.Unauthorized));

        public static Result Forbidden(string message) =>
            Result.Fail(new Error(403, message, ErrorType.Forbidden));

        public static Result InternalServerError(string message) =>
            Result.Fail(new Error(500, message, ErrorType.ServerError));

        public static Result Fail(IEnumerable<IError> errors) =>
           Result.Fail(errors);

        public static Result Ok() => Result.Ok();

    }
}
