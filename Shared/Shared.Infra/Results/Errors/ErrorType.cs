namespace Shared.Infra.Results.Errors;

public enum ErrorType
{
    None = 0,
    Validation = 1,
    NotFound = 2,
    Conflict = 3,
    Unauthorized = 4,
    Failure = 5,
    Forbidden = 6,
    ServerError = 7
}