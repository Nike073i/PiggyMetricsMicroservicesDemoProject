using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace NotificationService.Domain
{
    public class Recipient
    {
        [BsonId]
        public string AccountName { get; set; }

        [NotNull]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<NotificationKind, NotificationSettings> ScheduledNotifications { get; set; }

        public Recipient()
        {
            ScheduledNotifications = new Dictionary<NotificationKind, NotificationSettings>();
        }

        public override string ToString()
        {
            return $"Recipient{{accountName='{AccountName}', email='{Email}'}}";
        }
    }
}