using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Domain.Repositories;

public interface IExchangeRateRepository
{
    Task<IReadOnlyList<ExchangeRate>> GetAllAsync(CancellationToken cancellationToken);
    Task<ExchangeRate?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<ExchangeRate?> GetByPairAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken);
    Task AddAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken);
    void Remove(ExchangeRate exchangeRate);
}