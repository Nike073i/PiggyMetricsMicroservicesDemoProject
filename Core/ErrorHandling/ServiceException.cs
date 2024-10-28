using System;

namespace Core.ErrorHandling
{
    /// <summary>
    /// Исключение сервиса счетов
    /// </summary>
    public class ServiceException : Exception
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public ErrorCode ErrorCode { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="code">Код ошибки</param>
        public ServiceException(string message, ErrorCode code) : base(message)
        {
            ErrorCode = code;
        }

        /// <summary>
        /// Получить ошибку
        /// </summary>
        /// <returns></returns>
        public ServiceError GetError()
        {
            return new ServiceError { ErrorCode = ErrorCode, Message = Message };
        }
    }
}
