using CurrencyExchange.Application.DTOs;
using CurrencyExchange.Application.Exceptions;
using CurrencyExchange.Application.Interfaces;
using CurrencyExchange.Domain.Entities;
using CurrencyExchange.Domain.Repositories;

namespace CurrencyExchange.Application.Services;

public class ExchangeRateService(IExchangeRateRepository repository, IUnitOfWork unitOfWork) : IExchangeRateService
{
    public async Task<IReadOnlyList<ExchangeRateDto>> GetAllAsync(CancellationToken ct) =>
        (await repository.GetAllAsync(ct)).Select(ExchangeRateDto.From).ToList();

    public async Task<ExchangeRateDto> GetByIdAsync(int id, CancellationToken ct) =>
        ExchangeRateDto.From(await GetOrThrowAsync(id, ct));

    public async Task<ExchangeRateDto> CreateAsync(ExchangeRateRequest request, CancellationToken ct)
    {
        var entity = new ExchangeRate(request.FromCurrency, request.ToCurrency, request.Rate);

        return await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await EnsurePairIsFreeAsync(entity.FromCurrency, entity.ToCurrency, null, ct);
            await repository.AddAsync(entity, ct);
            await unitOfWork.SaveChangesAsync(ct);
            return ExchangeRateDto.From(entity);
        }, ct);
    }

    public Task<ExchangeRateDto> UpdateAsync(int id, ExchangeRateRequest request, CancellationToken ct) =>
        unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var entity = await GetOrThrowAsync(id, ct);
            entity.Update(request.FromCurrency, request.ToCurrency, request.Rate);
            await EnsurePairIsFreeAsync(entity.FromCurrency, entity.ToCurrency, id, ct);

            await unitOfWork.SaveChangesAsync(ct);
            return ExchangeRateDto.From(entity);
        }, ct);

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        repository.Remove(await GetOrThrowAsync(id, ct));
        await unitOfWork.SaveChangesAsync(ct);
    }

    private async Task<ExchangeRate> GetOrThrowAsync(int id, CancellationToken ct) =>
        await repository.GetByIdAsync(id, ct) ?? throw new NotFoundException($"No existe el tipo de cambio con id {id}.");

    private async Task EnsurePairIsFreeAsync(string from, string to, int? currentId, CancellationToken ct)
    {
        var existing = await repository.GetByPairAsync(from, to, ct);
        if (existing is not null && existing.Id != currentId)
            throw new ConflictException($"Ya existe un tipo de cambio {from} -> {to}.");
    }
}
