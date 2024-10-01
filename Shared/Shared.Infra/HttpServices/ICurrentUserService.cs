namespace Shared.Infra.HttpServices
{
    public interface ICurrentUserService
    {
        string Name { get; }
        string Id { get; }

    }
}
