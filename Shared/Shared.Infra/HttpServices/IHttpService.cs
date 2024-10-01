namespace Shared.Infra.HttpServices
{
    public interface IHttpService
    {
        HttpClient HttpClient { get; set; }
        Task<HttpResponseWrapper<object>> Delete(string url);
        Task<HttpResponseWrapper<T>> Get<T>(string url);
        Task<HttpResponseWrapper<T>> Get<T>(Uri url);
        Task<HttpResponseWrapper<string>> GetString(string url);
        Task<HttpResponseWrapper<object>> Post<T>(string url, T data);
        Task<HttpResponseWrapper<object>> Post<T>(Uri url, T data);
        Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data);
        Task<HttpResponseWrapper<object>> Put<T>(string url, T data);
        Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T data);

    }
}
