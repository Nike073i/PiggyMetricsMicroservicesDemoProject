namespace Core.ErrorHandling
{
    /// <summary>
    /// Ошибка запроса
    /// </summary>
    public class ServiceError
    {
        /// <summary>
        /// Описание ошибки
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Код ошибки
        /// </summary>
        public ErrorCode ErrorCode { get; set; }
    }
}
