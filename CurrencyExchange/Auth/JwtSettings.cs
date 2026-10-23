namespace CurrencyExchange.Auth;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = "CurrencyExchange";
    public string Audience { get; init; } = "CurrencyExchangeClients";
    public string Key { get; init; } = string.Empty;
    public int ExpirationMinutes { get; init; } = 60;
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
