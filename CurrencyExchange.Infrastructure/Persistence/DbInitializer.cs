using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (context.ExchangeRates.Any()) return;

        context.ExchangeRates.AddRange(
            new ExchangeRate("USD", "PEN", 3.75m),
            new ExchangeRate("PEN", "USD", 0.266667m),
            new ExchangeRate("EUR", "PEN", 4.05m));
        await context.SaveChangesAsync();
    }
}
