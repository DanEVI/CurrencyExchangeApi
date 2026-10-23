using CurrencyExchange.Domain.Entities;

namespace CurrencyExchange.Application.DTOs;

public record ExchangeRateDto(int Id, string FromCurrency, string ToCurrency, decimal Rate, DateTime UpdatedAtUtc)
{
    public static ExchangeRateDto From(ExchangeRate e) =>
        new(e.Id, e.FromCurrency, e.ToCurrency, e.Rate, e.UpdatedAtUtc);
}

public record ExchangeRateRequest(string FromCurrency, string ToCurrency, decimal Rate);

public record ConversionRequest(decimal Amount, string FromCurrency, string ToCurrency);

public record ConversionResponse(
    decimal Amount,
    decimal ConvertedAmount,
    string FromCurrency,
    string ToCurrency,
    decimal ExchangeRate);
