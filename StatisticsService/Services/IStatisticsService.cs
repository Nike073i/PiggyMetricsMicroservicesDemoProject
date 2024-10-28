using StatisticsService.Domain;
using StatisticsService.Domain.Timeseries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StatisticsService.Services
{
    /// <summary>
    /// Сервис статистики
    /// </summary>
    public interface IStatisticsService
    {
        /// <summary>
        /// Находит счет по имени
        /// </summary>
        /// <param name="accountName">Имя счета</param>
        /// <returns>Найденный аккаунт</returns>
        Task<List<DataPoint>> FindByAccountName(string accountName);

        /// <summary>
        /// Преобразует данные об аккаунте в статистике.
        /// Составной DataPointId перезаписывает объект
        /// для каждого аккаунта в течение суток.
        /// </summary>
        /// <param name="accountName">Имя аккаунта</param>
        /// <param name="account">Данные аккаунта</param>
        /// <returns>Статистика аккаунта</returns>
        Task<DataPoint> Save(string accountName, Account account);
    }
}