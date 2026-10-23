using CurrencyExchange.Application.DTOs;

namespace CurrencyExchange.Application.Interfaces;

public interface IExchangeRateService
{
    Task<IReadOnlyList<ExchangeRateDto>> GetAllAsync(CancellationToken ct);
    Task<ExchangeRateDto> GetByIdAsync(int id, CancellationToken ct);
    Task<ExchangeRateDto> CreateAsync(ExchangeRateRequest request, CancellationToken ct);
    Task<ExchangeRateDto> UpdateAsync(int id, ExchangeRateRequest request, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}

public interface IConversionService
{
    Task<ConversionResponse> ConvertAsync(ConversionRequest request, CancellationToken ct);
}
