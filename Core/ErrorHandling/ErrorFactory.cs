using System;
using System.Collections.Generic;

namespace Core.ErrorHandling
{
    /// <summary>
    /// Фабрика исключений
    /// </summary>
    public static class ErrorFactory
    {
        private static Dictionary<ErrorCode, Func<string, ServiceException>> _factoryMap = new Dictionary<ErrorCode, Func<string, ServiceException>>
        {
            {ErrorCode.NotFound, (msg) => new ServiceException( msg?? "Запрашиваемый объект не найден", ErrorCode.NotFound) },
            {ErrorCode.UnspecifiedError, (msg) => new ServiceException(msg?? "Неизвестная ошибка", ErrorCode.UnspecifiedError) },
            {ErrorCode.WrongRequest, (msg) => new ServiceException(msg?? "Неверный запрос", ErrorCode.WrongRequest) }
        };

        /// <summary>
        /// Создать исключение
        /// </summary>
        /// <param name="errorCode">Код ошибки</param>
        /// <param name="message">Текст ошибки</param>
        /// <returns></returns>
        public static ServiceException Create(ErrorCode errorCode, string message = null)
        {
            return _factoryMap[errorCode](message);
        }
    }
}
