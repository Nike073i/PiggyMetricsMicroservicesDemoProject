using AccountService.Client;
using AccountService.Domain;
using Core.ErrorHandling;
using AccountService.Extensions;
using AccountService.Repository;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AccountService.Services
{
    /// <summary>
    /// Управление счетами
    /// </summary>
    public class AccountService : IAccountService
    {
        /// <summary>
        /// Сервис логирования
        /// </summary>
        private static readonly ILogger _log = Log.ForContext<AccountService>();

        /// <summary>
        /// Хранилище счетов
        /// </summary>
        private readonly IAccountRepository _repository;

        /// <summary>
        /// Клиент сервиса статистики
        /// </summary>
        private IStatisticsServiceClient _statisticsClient;

        /// <summary>
        /// Клиент сервиса авторизации
        /// </summary>
        private readonly IAuthServiceClient _authClient;

        public AccountService(IAccountRepository repository,
            IAuthServiceClient authClient,
            IStatisticsServiceClient statisticsClient)
        {
            _repository = repository;
            _authClient = authClient;
            _statisticsClient = statisticsClient;
        }

        ///<inheritdoc/>
        public async Task<Account> Create(User user)
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            Account existing = await _repository.FindByName(user.Username);

            if (existing != null)
            {
                throw ErrorFactory.Create(ErrorCode.WrongRequest, $"Account already exists : {user.Username}");
            }

            await _authClient.CreateUserAsync(user);

            Saving saving = new Saving();
            saving.Amount = 0m;
            saving.Currency = CurrencyExtensions.GetDefault();
            saving.Interest = 0m;
            saving.Deposit = false;
            saving.Capitalization = false;

            Account account = new Account();
            account.Name = user.Username;
            account.LastSeen = utcNow;
            account.Saving = saving;

            await _repository.Save(account);

            _log.Information($"New account has been created: {account.Name}");

            return account;
        }

        ///<inheritdoc/>
        public async Task<Account> FindByName(string accountName)
        {
            return await _repository.FindByName(accountName);
        }

        ///<inheritdoc/>
        public async Task SaveChanges(string name, Account update)
        {
            DateTimeOffset utcNow = DateTimeOffset.UtcNow;

            Account account = await _repository.FindByName(name);

            if (account == null)
            {
                throw new Exception($"Can't find account with name {name}");
            }

            account.Incomes = update.Incomes;
            account.Expenses = update.Expenses;
            account.Saving = update.Saving;
            account.Note = update.Note;
            account.LastSeen = utcNow;
            await _repository.Save(account);

            _log.Debug($"Account {name} changes has been saved");

            await _statisticsClient.UpdateStatistics(name, account);
        }
    }
}
