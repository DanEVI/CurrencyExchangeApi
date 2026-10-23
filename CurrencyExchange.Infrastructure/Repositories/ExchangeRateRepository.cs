using CurrencyExchange.Domain.Entities;
using CurrencyExchange.Domain.Repositories;
using CurrencyExchange.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Infrastructure.Repositories;

public class ExchangeRateRepository(AppDbContext context) : IExchangeRateRepository
{
    public async Task<IReadOnlyList<ExchangeRate>> GetAllAsync(CancellationToken cancellationToken) =>
        await context.ExchangeRates.AsNoTracking().OrderBy(x => x.Id).ToListAsync(cancellationToken);

    public Task<ExchangeRate?> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        context.ExchangeRates.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<ExchangeRate?> GetByPairAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken)
    {
        var from = fromCurrency.Trim().ToUpperInvariant();
        var to = toCurrency.Trim().ToUpperInvariant();
        return context.ExchangeRates.FirstOrDefaultAsync(x => x.FromCurrency == from && x.ToCurrency == to, cancellationToken);
    }

    public async Task AddAsync(ExchangeRate exchangeRate, CancellationToken cancellationToken) =>
        await context.ExchangeRates.AddAsync(exchangeRate, cancellationToken);

    public void Remove(ExchangeRate exchangeRate) => context.ExchangeRates.Remove(exchangeRate);
}
