namespace Shared.Infra.Results.Errors;

public sealed record Error(int Code, string Message, ErrorType Type) : IError
{
    public static readonly Error None = new(0, string.Empty, ErrorType.None);
}