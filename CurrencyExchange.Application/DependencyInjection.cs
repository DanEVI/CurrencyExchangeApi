using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyExchange.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IExchangeRateService, ExchangeRateService>();
        services.AddScoped<IConversionService, ConversionService>();
        return services;
    }
}
