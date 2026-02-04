# Igreja Social - Dashboard

Painel administrativo para gestão de famílias atendidas, empréstimos de equipamentos e entregas de cestas básicas.

## Visão geral
- **Backend** em ASP.NET Core com EF Core, Identity e documentação via Swagger.
- **Frontend** em Blazor WebAssembly com MudBlazor.
- **Banco de dados** SQL Server (configurado em `DefaultConnection`).

## Arquitetura
```
src/
├── IgrejaSocial.API           # API + Identity + Swagger
├── IgrejaSocial.Application   # Serviços e DTOs
├── IgrejaSocial.Domain        # Entidades e regras de domínio
├── IgrejaSocial.Infrastructure# EF Core, repositórios e integrações externas
└── IgrejaSocial.Web           # Blazor WebAssembly + MudBlazor
```

## Requisitos
- .NET 8 SDK
- SQL Server (ou ajuste a connection string)

## Como rodar o projeto
1. Configure a connection string em `src/IgrejaSocial.API/appsettings.json` (chave `DefaultConnection`).
2. Execute as migrações (se aplicável) e rode a API:

```bash
dotnet run --project src/IgrejaSocial.API/IgrejaSocial.API.csproj
```

3. Acesse:
   - UI: `https://localhost:5001`
   - Swagger: `https://localhost:5001/swagger`

## Credenciais iniciais
Usuário administrador criado automaticamente no primeiro start:
- **Email:** admin@igreja.local
- **Senha:** Admin@1234

Para customizar esse usuário, ajuste os valores em `Program.cs` (API) antes do primeiro start ou remova o usuário no banco e reinicie a aplicação. O seeding também cria as roles **Administrador** e **Voluntário**.

## Relatórios mensais
No painel principal (Dashboard), utilize os botões:
- **Exportar atendimentos do mês**: gera um CSV com as famílias atendidas no mês selecionado.
- **Baixar KPIs em PDF**: emite o PDF com indicadores, incluindo giro de estoque e tempo médio de empréstimo.

Esses relatórios são protegidos por role **Administrador**.

## Testes
```bash
dotnet test
```

## Observações de segurança
- Roles disponíveis: **Administrador** e **Voluntário**.
- Endpoints sensíveis (ex.: relatórios e cadastro de equipamentos) exigem perfil de administrador.
