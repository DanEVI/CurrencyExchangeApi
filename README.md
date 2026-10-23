# Currency Exchange API

API de tipos de cambio en .NET 9 con EF Core (SQLite) y autenticación JWT.

## Ejecutar

Con Docker:
```bash
docker compose up --build
```

Sin Docker:
```bash
dotnet run --project CurrencyExchange --urls http://localhost:8080
```

La API queda en `http://localhost:8080`. Usuario de prueba: `admin` / `admin123`.

## Endpoints

Todos, excepto el login, requieren el header `Authorization: Bearer <token>`.

| Método | Ruta | Descripción |
|---|---|---|
| POST | `/api/auth/login` | Devuelve el token JWT |
| GET | `/api/exchange-rates` | Lista los tipos de cambio |
| GET | `/api/exchange-rates/{id}` | Obtiene uno |
| POST | `/api/exchange-rates` | Crea uno |
| PUT | `/api/exchange-rates/{id}` | Actualiza uno |
| DELETE | `/api/exchange-rates/{id}` | Elimina uno |
| POST | `/api/conversion` | Aplica un tipo de cambio a un monto |

### Login
```json
POST /api/auth/login
{ "username": "admin", "password": "admin123" }
```

### Crear o actualizar un tipo de cambio
```json
POST /api/exchange-rates
{ "fromCurrency": "USD", "toCurrency": "PEN", "rate": 3.75 }
```

### Convertir un monto
```json
POST /api/conversion
{ "amount": 100, "fromCurrency": "USD", "toCurrency": "PEN" }

200 OK
{ "amount": 100, "convertedAmount": 375.00, "fromCurrency": "USD", "toCurrency": "PEN", "exchangeRate": 3.75 }
```

## Errores

| Código | Cuándo |
|---|---|
| 400 | Datos inválidos |
| 401 | Falta el token o es inválido |
| 404 | No existe el registro o el par de monedas |
| 409 | Ya existe un tipo de cambio para ese par |
| 500 | Error interno |

