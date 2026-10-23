using CurrencyExchange.Application.DTOs;
using CurrencyExchange.Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CurrencyExchange.Endpoints;

public static class ConversionEndpoints
{
    public static IEndpointRouteBuilder MapConversionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/conversion").WithTags("Conversion").RequireAuthorization();

        group.MapPost("/", Convert);

        return app;
    }

    private static async Task<Ok<ConversionResponse>> Convert(
        ConversionRequest request, IConversionService service, CancellationToken ct) =>
        TypedResults.Ok(await service.ConvertAsync(request, ct));
}
