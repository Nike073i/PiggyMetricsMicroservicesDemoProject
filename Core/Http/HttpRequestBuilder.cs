using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Core.Http
{
    /// <summary>
    /// Конструктор http-запросов
    /// </summary>
    public class HttpRequestBuilder
    {
        private HttpMethod _method = null;
        private string _requestUri = "";
        private HttpContent _content = null;
        private string _bearerToken = "";
        private string _acceptHeader = null;

        /// <summary>
        /// Устанавливает http-метод
        /// </summary>
        /// <param name="method">Http-метод</param>
        /// <returns>Конструктор http-запросов</returns>
        public HttpRequestBuilder AddMethod(HttpMethod method)
        {
            _method = method;
            return this;
        }

        /// <summary>
        /// Устанавливает uri запроса
        /// </summary>
        /// <param name="method">Uri запроса</param>
        /// <returns>Конструктор http-запросов</returns>
        public HttpRequestBuilder AddRequestUri(string requestUri)
        {
            _requestUri = requestUri;
            return this;
        }

        /// <summary>
        /// Устанавливает тело запроса
        /// </summary>
        /// <param name="content">Тело запроса</param>
        /// <returns>Конструктор http-запросов</returns>
        public HttpRequestBuilder AddContent(HttpContent content)
        {
            _content = content;
            return this;
        }

        /// <summary>
        /// Устанавливает токен авторизации
        /// </summary>
        /// <param name="bearerToken">Токен авторизации</param>
        /// <returns>Конструктор http-запросов</returns>
        public HttpRequestBuilder AddBearerToken(string bearerToken)
        {
            _bearerToken = bearerToken;
            return this;
        }

        /// <summary>
        /// Добавить заголовок возвращаемого контента
        /// </summary>
        /// <param name="acceptHeader">Заголовок возвращаемого контента</param>
        /// <returns>Конструктор http-запросов</returns>
        public HttpRequestBuilder AddAcceptHeader(string acceptHeader)
        {
            _acceptHeader = acceptHeader;
            return this;
        }

        /// <summary>
        /// Создать http запрос
        /// </summary>
        /// <returns>Http запрос</returns>
        public HttpRequestMessage Build()
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = _method,
                RequestUri = new Uri(_requestUri)
            };

            if (_content != null)
            {
                request.Content = _content;
            }

            if (!string.IsNullOrEmpty(_bearerToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            }

            request.Headers.Accept.Clear();

            if (!string.IsNullOrEmpty(_acceptHeader))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(_acceptHeader));
            }

            return request;
        }
    }
}
