using CurrencyExchange.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;

namespace CurrencyExchange.Endpoints;

public record LoginRequest(string Username, string Password);

public record LoginResponse(string Token, DateTime ExpiresAtUtc);

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth").AllowAnonymous();

        group.MapPost("/login", Login);

        return app;
    }

    private static Results<Ok<LoginResponse>, ProblemHttpResult> Login(
        LoginRequest request, TokenService tokenService, IOptions<JwtSettings> options)
    {
        var settings = options.Value;
        if (request.Username != settings.Username || request.Password != settings.Password)
            return TypedResults.Problem(title: "Credenciales inválidas", statusCode: StatusCodes.Status401Unauthorized);

        var (token, expires) = tokenService.Create(request.Username);
        return TypedResults.Ok(new LoginResponse(token, expires));
    }
}
