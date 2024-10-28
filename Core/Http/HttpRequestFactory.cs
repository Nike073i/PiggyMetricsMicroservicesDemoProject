using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Core.Http
{
    /// <summary>
    /// Фабрика http-запросов
    /// </summary>
    public static class HttpRequestFactory
    {
        /// <summary>
        /// Создание GET-запроса
        /// </summary>
        /// <param name="requestUri">Uri запроса</param>
        /// <param name="bearerToken">Токен авторизации</param>
        /// <returns>Http-запроса</returns>
        public static HttpRequestMessage Get(string requestUri, string bearerToken = null)
        {
            HttpRequestBuilder builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri);

            if (!string.IsNullOrWhiteSpace(bearerToken))
            {
                builder.AddBearerToken(bearerToken);
            }

            return builder.Build();
        }

        /// <summary>
        /// Создание POST-запроса
        /// </summary>
        /// <param name="requestUri">Uri запроса</param>
        /// <param name="value">Тело запроса</param>
        /// <param name="bearerToken">Токен авторизации</param>
        /// <returns>Http-запроса</returns>
        public static HttpRequestMessage Post(string requestUri, 
            object value = null,
            string bearerToken = null)
        {
            HttpRequestBuilder builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Post)
                .AddRequestUri(requestUri);

            if (value != null)
            {
                builder.AddContent(new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json"));
            }

            if (!string.IsNullOrWhiteSpace(bearerToken))
            {
                builder.AddBearerToken(bearerToken);
            }

            return builder.Build();
        }

        /// <summary>
        /// Создание Put-запроса
        /// </summary>
        /// <param name="requestUri">Uri запроса</param>
        /// <param name="value">Тело запроса</param>
        /// <param name="bearerToken">Токен авторизации</param>
        /// <returns>Http-запроса</returns>
        public static HttpRequestMessage Put(string requestUri,
            object value = null,
            string bearerToken = null)
        {
            HttpRequestBuilder builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Put)
                .AddRequestUri(requestUri);

            if (value != null)
            {
                builder.AddContent(new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json"));
            }

            if (!string.IsNullOrWhiteSpace(bearerToken))
            {
                builder.AddBearerToken(bearerToken);
            }

            return builder.Build();
        }
    }
}
