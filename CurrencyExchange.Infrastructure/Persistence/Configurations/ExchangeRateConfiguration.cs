using CurrencyExchange.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyExchange.Infrastructure.Persistence.Configurations;

public class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FromCurrency).IsRequired().HasMaxLength(3);
        builder.Property(x => x.ToCurrency).IsRequired().HasMaxLength(3);
        builder.Property(x => x.Rate).HasPrecision(18, 6);
        builder.HasIndex(x => new { x.FromCurrency, x.ToCurrency }).IsUnique();
    }
}
