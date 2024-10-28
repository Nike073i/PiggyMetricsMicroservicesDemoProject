using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AuthService.Domain
{
    public class User
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTimeOffset? Created { get; set; }

        /// <summary>
        /// Дата обновления
        /// </summary>
        public DateTimeOffset? Updated { get; set; }

        /// <summary>
        /// Дата удаления
        /// </summary>
        public DateTimeOffset? Deleted { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        /// <summary>
        /// Количество неудачных попыток входа
        /// </summary>
        public int FailedAttempts { get; set; }

        /// <summary>
        /// Время окончания блокировки
        /// </summary>
        public DateTimeOffset? LockoutEndDate { get; set; }
    }
}
