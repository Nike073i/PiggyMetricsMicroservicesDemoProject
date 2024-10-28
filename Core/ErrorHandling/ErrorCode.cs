namespace Core.ErrorHandling
{
    /// <summary>
    /// Коды ошибок
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// Неизвестная ошибка
        /// </summary>
        UnspecifiedError = 1,

        /// <summary>
        /// Сущность не найдена
        /// </summary>
        NotFound,

        /// <summary>
        /// Неверный запрос
        /// </summary>
        WrongRequest
    }
}
