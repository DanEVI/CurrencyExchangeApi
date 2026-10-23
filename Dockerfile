FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY CurrencyExchange.Domain/*.csproj CurrencyExchange.Domain/
COPY CurrencyExchange.Application/*.csproj CurrencyExchange.Application/
COPY CurrencyExchange.Infrastructure/*.csproj CurrencyExchange.Infrastructure/
COPY CurrencyExchange/*.csproj CurrencyExchange/
RUN dotnet restore CurrencyExchange/CurrencyExchange.csproj

COPY CurrencyExchange.Domain/ CurrencyExchange.Domain/
COPY CurrencyExchange.Application/ CurrencyExchange.Application/
COPY CurrencyExchange.Infrastructure/ CurrencyExchange.Infrastructure/
COPY CurrencyExchange/ CurrencyExchange/
RUN dotnet publish CurrencyExchange/CurrencyExchange.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

RUN mkdir /data && chown $APP_UID /data

ENV ASPNETCORE_URLS=http://+:8080
ENV ConnectionStrings__Default="Data Source=/data/currency.db"
EXPOSE 8080
USER $APP_UID
ENTRYPOINT ["dotnet", "CurrencyExchange.dll"]
