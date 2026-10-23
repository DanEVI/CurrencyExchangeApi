namespace CurrencyExchange.Domain.Entities;

public sealed class ExchangeRate
{
    public int Id { get; private set; }
    public string FromCurrency { get; private set; } = null!;
    public string ToCurrency { get; private set; } = null!;
    public decimal Rate { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    private ExchangeRate(){}
    
    public ExchangeRate(string fromCurrency, string toCurrency, decimal rate)
    {
        Update(fromCurrency, toCurrency, rate);
    }

    public void Update(string fromCurrency, string toCurrency, decimal rate)
    {
        if(string.IsNullOrWhiteSpace(fromCurrency) || string.IsNullOrWhiteSpace(toCurrency)) throw new ArgumentException("La moneda no puede estar vacía");
        
        var from = fromCurrency.Trim().ToUpperInvariant();
        var to = toCurrency.Trim().ToUpperInvariant();
        
        if(from.Length != 3 || to.Length != 3) throw new ArgumentException("longitud de moneda no válida");
        if (from == to) throw new ArgumentException("La moneda origen y destino deben ser diferentes");
        if (rate <= 0) throw new ArgumentException("La tasa debe ser mayor que cero.");
        FromCurrency = from;
        ToCurrency = to;
        Rate = rate;
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    public decimal Apply(decimal amount) => amount * Rate;
}