namespace Shared.Infra.Results.Errors;

public interface IError
{
    int Code { get; init; }
    string Message { get; init; }
    public ErrorType Type { get; init; }
}