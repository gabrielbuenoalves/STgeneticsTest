# Good Hamburger - Desafio Tecnico C#

Implementacao do desafio da lanchonete Good Hamburger usando .NET, MVC, Blazor Server e SQL Server.

# Stack e arquitetura

- .NET 8 (ASP.NET Core)
- Monolito modular com separacao por camadas:
  - `GoodHamburger.Domain`: entidades e regras de negocio
  - `GoodHamburger.Application`: casos de uso, DTOs e contratos
  - `GoodHamburger.Infrastructure`: EF Core, repositorios e migrations
  - `GoodHamburger.Web`: host web com API REST, MVC e Blazor Server
  - `GoodHamburger.Tests.Unit`: testes unitarios das regras
- EF Core com SQL Server e migrations
- Middleware global para tratamento centralizado de erros

# Requisitos atendidos

- API REST em C# / ASP.NET Core
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
- Validacao de itens duplicados (1 por categoria por pedido)
- Frontend em Blazor Server consumindo a propria API
- MVC com Controller + View de entrada
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

# 3) Aplicar migrations (opcional, a API tambem aplica no startup)

```bash
dotnet tool restore
dotnet tool run dotnet-ef database update -p src/GoodHamburger.Infrastructure/GoodHamburger.Infrastructure.csproj -s src/GoodHamburger.Web/GoodHamburger.Web.csproj
```

# 4) Rodar a aplicacao

```bash
dotnet run --project src/GoodHamburger.Web/GoodHamburger.Web.csproj
```

Acessos:

- Home MVC: `https://localhost:5001/` ou URL exibida no console
- Swagger: `/swagger`
- Blazor Cardapio: `/blazor/menu`
- Blazor Pedidos: `/blazor/orders`

## Testes

```bash
dotnet test
```

## Tratamento de erros

Middleware global captura excecoes de dominio e recursos nao encontrados, retornando payload JSON padronizado:

```json
{
  "title": "Erro de validacao de negocio",
  "status": 400,
  "detail": "Itens duplicados nao permitidos: apenas um sanduiche por pedido."
}
```
