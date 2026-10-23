using CurrencyExchange.Application.DTOs;
using CurrencyExchange.Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CurrencyExchange.Endpoints;

public static class ExchangeRateEndpoints
{
    public static IEndpointRouteBuilder MapExchangeRateEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/exchange-rates").WithTags("Exchange rates").RequireAuthorization();

        group.MapGet("/", GetAll);
        group.MapGet("/{id:int}", GetById).WithName(nameof(GetById));
        group.MapPost("/", Create);
        group.MapPut("/{id:int}", Update);
        group.MapDelete("/{id:int}", Delete);

        return app;
    }

    private static async Task<Ok<IReadOnlyList<ExchangeRateDto>>> GetAll(
        IExchangeRateService service, CancellationToken ct) =>
        TypedResults.Ok(await service.GetAllAsync(ct));

    private static async Task<Ok<ExchangeRateDto>> GetById(
        int id, IExchangeRateService service, CancellationToken ct) =>
        TypedResults.Ok(await service.GetByIdAsync(id, ct));

    private static async Task<CreatedAtRoute<ExchangeRateDto>> Create(
        ExchangeRateRequest request, IExchangeRateService service, CancellationToken ct)
    {
        var created = await service.CreateAsync(request, ct);
        return TypedResults.CreatedAtRoute(created, nameof(GetById), new { id = created.Id });
    }

    private static async Task<Ok<ExchangeRateDto>> Update(
        int id, ExchangeRateRequest request, IExchangeRateService service, CancellationToken ct) =>
        TypedResults.Ok(await service.UpdateAsync(id, request, ct));

    private static async Task<NoContent> Delete(
        int id, IExchangeRateService service, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return TypedResults.NoContent();
    }
}
