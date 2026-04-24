# Good Hamburger - Desafio Técnico C#

Aplicação para o desafio da lanchonete Good Hamburger usando .NET 10, API REST separada, frontend Blazor Server e SQL Server em Docker.

## Resumo

O projeto foi dividido em camadas e em dois hosts independentes:

- `GoodHamburger.Domain`: regras de negócio e entidades
- `GoodHamburger.Application`: casos de uso, DTOs e contratos
- `GoodHamburger.Infrastructure`: EF Core, repositórios, migrations e seed
- `GoodHamburger.Api`: host exclusivo da API REST
- `GoodHamburger.Web`: host exclusivo do frontend Blazor Server
- `GoodHamburger.Tests.Unit`: testes unitários das regras de negócio

O frontend não acessa banco nem expõe endpoints próprios. Ele consome a API por `HttpClient` com a URL configurada em `ApiBaseUrl`.

## O que está entregue

- API REST separada do frontend
- CRUD completo de pedidos
  - `POST /api/orders`
  - `GET /api/orders`
  - `GET /api/orders/{id}`
  - `PUT /api/orders/{id}`
  - `DELETE /api/orders/{id}`
- Endpoint de cardápio
  - `GET /api/menu`
- Cálculo de subtotal, desconto e total final
- Regras de desconto para combinações de sanduíche, batata e refrigerante
- Validação de itens duplicados no backend
- Validação no frontend para impedir pedidos inválidos
- Testes unitários das regras de negócio

## Regras principais

- cada pedido aceita no máximo 1 item por categoria
- é obrigatório selecionar um sanduíche
- itens inexistentes no cardápio retornam erro claro
- itens duplicados retornam erro claro

## Requisitos para rodar

- .NET SDK 10.0.100 ou superior, compatível com `global.json`
- Docker Desktop ou Docker Engine com Docker Compose
- certificado HTTPS de desenvolvimento confiável no sistema

Se o navegador bloquear HTTPS local na primeira execução, rode:

```bash
dotnet dev-certs https --trust
```

## Como executar

1. Suba o SQL Server no Docker:

```bash
docker compose up -d
```

2. Restaure as dependências:

```bash
dotnet restore
```

3. Rode a API em um terminal:

```bash
dotnet run --project src/GoodHamburger.Api/GoodHamburger.Api.csproj --launch-profile https
```

4. Rode o frontend em outro terminal:

```bash
dotnet run --project src/GoodHamburger.Web/GoodHamburger.Web.csproj --launch-profile https
```

## URLs locais

- API HTTPS: `https://localhost:7122`
- Swagger da API: `https://localhost:7122/swagger`
- Frontend HTTPS: `https://localhost:7121`
- Cardápio: `https://localhost:7121/blazor/menu`
- Pedidos: `https://localhost:7121/blazor/orders`

## Configuração

- A API lê a connection string em `src/GoodHamburger.Api/appsettings.json`
- O frontend lê a URL da API em `src/GoodHamburger.Web/appsettings.Development.json`
- As portas estão em `src/GoodHamburger.Api/Properties/launchSettings.json` e `src/GoodHamburger.Web/Properties/launchSettings.json`
- Se mudar as portas, atualize também `ApiBaseUrl` no frontend e `FrontendBaseUrl` na API

## Testes

```bash
dotnet test
```

## Tratamento de erros

A API usa middleware global para padronizar erros de domínio e recursos não encontrados. Exemplo de resposta:

```json
{
  "title": "Erro de validação de negócio",
  "status": 400,
  "detail": "Itens duplicados não permitidos: apenas um sanduíche por pedido."
}
```

## Observações

- O SQL Server do Docker usa a senha definida em `docker-compose.yml` e nas connection strings do projeto.
- O Swagger fica disponível apenas no host da API.
- O frontend Blazor apenas consome a API.
