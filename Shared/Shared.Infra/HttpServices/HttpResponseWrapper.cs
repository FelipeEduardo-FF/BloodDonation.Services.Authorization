namespace Shared.Infra.HttpServices
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T response, bool sucess, HttpResponseMessage httpResponseMessage)
        {
            Sucess = sucess;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        
        }
        public bool Sucess { get; set; }
        public T Response { get; set; }
        public HttpResponseMessage  HttpResponseMessage { get; set; }

        public async Task<string> getBody() {
            return await HttpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
