# Good Hamburger - Desafio Tecnico C#

Implementacao do desafio da lanchonete Good Hamburger usando .NET 10, Blazor Server, API REST separada e SQL Server.

# Stack e arquitetura

- .NET 10 (ASP.NET Core)
- Arquitetura em camadas:
  - `GoodHamburger.Domain`: entidades e regras de negocio
  - `GoodHamburger.Application`: casos de uso, DTOs e contratos
  - `GoodHamburger.Infrastructure`: EF Core, repositorios e migrations
  - `GoodHamburger.Api`: host exclusivo da API REST
  - `GoodHamburger.Web`: host exclusivo do frontend Blazor Server
  - `GoodHamburger.Tests.Unit`: testes unitarios das regras
- EF Core com SQL Server e migrations
- Middleware global de tratamento de erros na API

# Requisitos atendidos

- API REST em C# / ASP.NET Core separada do frontend
- CRUD completo de pedidos:
  - `POST /api/orders`
  - `GET /api/orders`
  - `GET /api/orders/{id}`
  - `PUT /api/orders/{id}`
  - `DELETE /api/orders/{id}`
- Endpoint de cardapio:
  - `GET /api/menu`
- Calculo de subtotal, desconto e total final
- Regras de desconto:
  - Sanduiche + batata + refrigerante: 20%
  - Sanduiche + refrigerante: 15%
  - Sanduiche + batata: 10%
- Validacao de itens duplicados no backend (1 por categoria por pedido)
- Validacao no frontend para impedir envio sem item e sem sanduiche
- Frontend Blazor consumindo API via `HttpClient` com `ApiBaseUrl` configuravel
- Testes unitarios das regras de negocio

# Regras de negocio implementadas

- Cada pedido aceita no maximo:
  - 1 sanduiche
  - 1 batata
  - 1 refrigerante
- Pedido sem sanduiche e considerado invalido
- Itens duplicados retornam erro claro
- Itens inexistentes no cardapio retornam erro claro

# Executando localmente

# 1) Subir o SQL Server no Docker

```bash
docker compose up -d
```

# 2) Restaurar dependencias

```bash
dotnet restore
```

# 3) Confiar no certificado HTTPS de desenvolvimento (macOS/Linux/Windows)

```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

## 4) Rodar a API (porta dedicada)

```bash
dotnet run --project src/GoodHamburger.Api/GoodHamburger.Api.csproj --launch-profile https
```

# 5) Rodar o frontend Blazor (porta dedicada)

```bash
dotnet run --project src/GoodHamburger.Web/GoodHamburger.Web.csproj --launch-profile https
```

# Acessos locais

- API HTTPS: `https://localhost:7122`
- Swagger API: `https://localhost:7122/swagger`
- Frontend Blazor HTTPS: `https://localhost:7121`
- Tela de cardapio: `https://localhost:7121/blazor/menu`
- Tela de pedidos: `https://localhost:7121/blazor/orders`

Observacao: essas portas sao apenas a configuracao atual do projeto. Se quiser, altere em `Properties/launchSettings.json` de cada host.
O frontend usa `ApiBaseUrl` em `src/GoodHamburger.Web/appsettings.json`.

# Testes

```bash
dotnet test
```

## Tratamento de erros

Middleware global na API captura excecoes de dominio e recursos nao encontrados, retornando payload JSON padronizado:

```json
{
  "title": "Erro de validacao de negocio",
  "status": 400,
  "detail": "Itens duplicados nao permitidos: apenas um sanduiche por pedido."
}
```
