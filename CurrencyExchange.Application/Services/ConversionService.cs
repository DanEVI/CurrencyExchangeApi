using CurrencyExchange.Application.DTOs;
using CurrencyExchange.Application.Exceptions;
using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Domain.Repositories;

namespace CurrencyExchange.Application.Services;

public class ConversionService(IExchangeRateRepository repository) : IConversionService
{
    public async Task<ConversionResponse> ConvertAsync(ConversionRequest request, CancellationToken ct)
    {
        if (request.Amount <= 0)
            throw new ArgumentException("El monto debe ser mayor que cero.");
        if (string.IsNullOrWhiteSpace(request.FromCurrency) || string.IsNullOrWhiteSpace(request.ToCurrency))
            throw new ArgumentException("La moneda origen y destino son obligatorias.");

        var rate = await repository.GetByPairAsync(request.FromCurrency, request.ToCurrency, ct)
                   ?? throw new NotFoundException(
                       $"No hay tipo de cambio registrado para {request.FromCurrency.Trim().ToUpperInvariant()} -> {request.ToCurrency.Trim().ToUpperInvariant()}.");

        var converted = Math.Round(rate.Apply(request.Amount), 2, MidpointRounding.AwayFromZero);

        return new ConversionResponse(request.Amount, converted, rate.FromCurrency, rate.ToCurrency, rate.Rate);
    }
}
