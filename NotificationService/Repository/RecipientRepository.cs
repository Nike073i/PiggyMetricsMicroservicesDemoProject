using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Core.Configuration;
using Core.Repository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NotificationService.Domain;

namespace NotificationService.Repository
{
    public class RecipientRepository: MongoRepositoryBase<Recipient>, IRecipientRepository
    {
        public RecipientRepository(IOptions<MongoSettings> configuration) 
            : base(configuration.Value.CollectionName, configuration.Value.ConnectionString)
        {
        }

        public async Task<Recipient> FindAsync(string accountName, CancellationToken token = default)
        {
            IAsyncCursor<Recipient> queryResult = await Collection
                .FindAsync(Builders<Recipient>.Filter.Eq(x => x.AccountName, accountName)
                    , cancellationToken: token);

            return await queryResult.FirstOrDefaultAsync(token);
        }

        public async Task<List<Recipient>> FindReadyForBackupAsync(CancellationToken token = default)
        {
            IAsyncCursor<Recipient> cursor = await Collection
                .FindAsync(BuildReadyRecipientFilter(NotificationKind.Backup),
                    cancellationToken: token);

            return await cursor.ToListAsync(token);
        }

        public async Task<List<Recipient>> FindReadyForRemindAsync(CancellationToken token = default)
        {
            IAsyncCursor<Recipient> cursor = await Collection
                .FindAsync(BuildReadyRecipientFilter(NotificationKind.Remind),
                    cancellationToken: token);

            return await cursor.ToListAsync(token);
        }

        public async Task<Recipient> SaveAsync(Recipient recipient, CancellationToken token = default)
        {
            var options = new FindOneAndUpdateOptions<Recipient>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<Recipient>();
            var updateDefinitions = new List<UpdateDefinition<Recipient>>
            {
                updateDefinitionBuilder.Set(x => x.AccountName, recipient.AccountName),
                updateDefinitionBuilder.Set(x => x.Email, recipient.Email),
                updateDefinitionBuilder.Set(x => x.ScheduledNotifications, recipient.ScheduledNotifications),
            };

            Recipient updated =  await Collection.FindOneAndUpdateAsync(
                Builders<Recipient>.Filter.Eq(x => x.AccountName, recipient.AccountName),
                updateDefinitionBuilder.Combine(updateDefinitions),
                options,
                token);

            return updated;
        }

        Expression<Func<Recipient, bool>> BuildReadyRecipientFilter(NotificationKind kind)
        {
            DateTime dtWeek = DateTime.Now.AddDays(-1 *(int) Frequency.Weekly);

            DateTime dtMonth = DateTime.Now.AddDays(-1 *(int) Frequency.Monthly);

            DateTime dtQuarter = DateTime.Now.AddDays(-1 *(int) Frequency.Quarterly);

            return x => x.ScheduledNotifications
                .Any(y => y.Key == kind
                          && y.Value.IsActive == true
                          && (y .Value.Frequency == Frequency.Monthly 
                                    && y .Value.LastNotified < dtMonth
                              || y .Value.Frequency == Frequency.Weekly 
                                    && y .Value.LastNotified < dtWeek
                              || y .Value.Frequency == Frequency.Quarterly 
                                    && y .Value.LastNotified < dtQuarter
                              || y.Value.LastNotified == null));                                                      
        }
    }
}
