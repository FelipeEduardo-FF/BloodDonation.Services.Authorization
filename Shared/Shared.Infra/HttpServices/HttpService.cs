using Shared.Infra.Exceptions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Shared.Infra.HttpServices
{
    public class HttpService :IHttpService
    {
        public HttpClient HttpClient { get; set; }
        private static readonly JsonSerializerOptions defaultJsonSerializerOptions = (new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    
        public HttpService(HttpClient HttpClient)
        {
            this.HttpClient = HttpClient;        
        }

        public async Task<HttpResponseWrapper<object>> Post<T>(Uri url, T data)
        {
            var dataJson = System.Text.Json.JsonSerializer.Serialize(data, defaultJsonSerializerOptions);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
                await CheckInvalidStatusCodeAsync(response);

            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);

        }


        public async Task<HttpResponseWrapper<object>> Post<T>(string url, T data)
        {

            var dataJson = System.Text.Json.JsonSerializer.Serialize(data, defaultJsonSerializerOptions);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
                await CheckInvalidStatusCodeAsync(response);

            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Tipo do objeto enviado</typeparam>
        /// <typeparam name="TResponse">Tipo da resposta Esperada</typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<HttpResponseWrapper<TResponse>> Post<T, TResponse>(string url, T data)
        {
            var dataJson = System.Text.Json.JsonSerializer.Serialize(data, defaultJsonSerializerOptions);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var respJson = await HttpClient.PostAsync(url, stringContent);

            if (!respJson.IsSuccessStatusCode)
                await CheckInvalidStatusCodeAsync(respJson);

            var resposta = await Deserialize<TResponse>(respJson, defaultJsonSerializerOptions);
            return new HttpResponseWrapper<TResponse>(resposta, respJson.IsSuccessStatusCode, respJson);

        }

        public async Task<HttpResponseWrapper<T>> Get<T>(string url)
        {
            var respostaHttp = await HttpClient.GetAsync(url);

            if (!respostaHttp.IsSuccessStatusCode)
                await CheckInvalidStatusCodeAsync(respostaHttp);

            var resposta = await Deserialize<T>(respostaHttp, defaultJsonSerializerOptions);
            return new HttpResponseWrapper<T>(resposta, true, respostaHttp);

        }
        public async Task<HttpResponseWrapper<string>> GetString(string url)
        {
            var respostaHttp = await HttpClient.GetAsync(url);

            if (!respostaHttp.IsSuccessStatusCode)
                await CheckInvalidStatusCodeAsync(respostaHttp);

            var resposta = await respostaHttp.Content.ReadAsStringAsync().ConfigureAwait(false);
            return new HttpResponseWrapper<string>(resposta, true, respostaHttp);

        }
        public async Task<HttpResponseWrapper<T>> Get<T>(Uri url)
        {
            var respostaHttp = await HttpClient.GetAsync(url);

            if (!respostaHttp.IsSuccessStatusCode)
                await CheckInvalidStatusCodeAsync(respostaHttp);

            var resposta = await Deserialize<T>(respostaHttp, defaultJsonSerializerOptions).ConfigureAwait(false);
            return new HttpResponseWrapper<T>(resposta, true, respostaHttp);

        }

        public async Task<HttpResponseWrapper<object>> Put<T>(string url, T data)
        {
            var dataJson = System.Text.Json.JsonSerializer.Serialize(data, defaultJsonSerializerOptions);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await HttpClient.PutAsync(url, stringContent);

            if (!response.IsSuccessStatusCode)
                await CheckInvalidStatusCodeAsync(response);

            return new HttpResponseWrapper<object>(null, response.IsSuccessStatusCode, response);
        }

        public async Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T data)
        {
            var dataJson = System.Text.Json.JsonSerializer.Serialize(data, defaultJsonSerializerOptions);
            var stringContent = new StringContent(dataJson, Encoding.UTF8, "application/json");
            var response = await HttpClient.PutAsync(url, stringContent);


            if (!response.IsSuccessStatusCode)
                await CheckInvalidStatusCodeAsync(response);

            var resposta = await Deserialize<TResponse>(response, defaultJsonSerializerOptions);
            return new HttpResponseWrapper<TResponse>(resposta, true, response);

        }




        public async Task<HttpResponseWrapper<object>> Delete(string url)
        {
            var responseHTTP = await HttpClient.DeleteAsync(url);

            if (!responseHTTP.IsSuccessStatusCode)
                await CheckInvalidStatusCodeAsync(responseHTTP);

            return new HttpResponseWrapper<object>(null, responseHTTP.IsSuccessStatusCode, responseHTTP);
        }
        private async Task<T> Deserialize<T>(HttpResponseMessage httpResponse, JsonSerializerOptions options=null)
        {
            var body = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false); 

            
            return JsonSerializer.Deserialize<T>(body, options);
        }

        private async Task CheckInvalidStatusCodeAsync(HttpResponseMessage response)
        {

            string MessageError = await response.Content.ReadAsStringAsync();
            switch (response.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new NotFoundException("Not Found", MessageError);
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException("Not Authorized");
                case HttpStatusCode.Forbidden:
                    throw new ForbiddenExcept("User not authorized", MessageError);
                case HttpStatusCode.BadRequest:
                    throw new BadRequestException("Invalid Request", MessageError);
                case HttpStatusCode.PaymentRequired:
                    throw new PaymentRequiredException("Payment Denied", MessageError);
                default:
                    throw new Exception(!string.IsNullOrWhiteSpace(MessageError) ? MessageError : "Unidentified error");
            }

        }


    }
}
