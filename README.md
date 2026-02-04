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
1. Configure a connection string usando variáveis de ambiente ou user-secrets:
   - `ConnectionStrings__DefaultConnection`
2. Execute as migrações (se aplicável) e rode a API:

```bash
dotnet run --project src/IgrejaSocial.API/IgrejaSocial.API.csproj
```

3. Acesse:
   - UI: `https://localhost:5001`
   - Swagger: `https://localhost:5001/swagger`

## Configuração de secrets (conexão e admin seed)
- **Connection string**: defina `ConnectionStrings__DefaultConnection` no ambiente ou com user-secrets.
- **Admin seed** (opcional): para criar um usuário administrador no primeiro start, configure:
  - `AdminSeed__Email`
  - `AdminSeed__Password`

> O seeding continua criando as roles **Administrador** e **Voluntário**.

## Relatórios mensais
No painel principal (Dashboard), utilize os botões:
- **Exportar atendimentos do mês**: gera um CSV com as famílias atendidas no mês selecionado.
- **Baixar KPIs em PDF**: emite o PDF com indicadores, incluindo giro de estoque e tempo médio de empréstimo.

Esses relatórios são protegidos por role **Administrador**.

## Testes
```bash
dotnet test
```

## Manutenção: adicionando novos tipos de equipamentos
1. Atualize o enum `TipoEquipamento` em `src/IgrejaSocial.Domain/Enums/EquipamentoEnums.cs`.
2. Caso use dados seed, acrescente o novo tipo em `IgrejaSocialDbContext` (se aplicável).
3. A UI lista os tipos automaticamente via `Enum.GetValues<TipoEquipamento>()`, então não há ajustes adicionais na tela.

## Backup inicial
Scripts prontos em `scripts/`:
- **SQLite**: define `DB_ENGINE=sqlite` e `SQLITE_PATH=/caminho/IgrejaSocial.db`.
- **SQL Server**: define `DB_ENGINE=sqlserver` e as variáveis `SQLSERVER_SERVER`, `SQLSERVER_DATABASE`, `SQLSERVER_USER`, `SQLSERVER_PASSWORD`.

Exemplos:
```bash
DB_ENGINE=sqlite SQLITE_PATH=./IgrejaSocial.db ./scripts/backup-db.sh
```

```bash
DB_ENGINE=sqlserver SQLSERVER_SERVER=localhost SQLSERVER_DATABASE=IgrejaSocialDb \
SQLSERVER_USER=sa SQLSERVER_PASSWORD='senha' ./scripts/backup-db.sh
```

## Observações de segurança
- Roles disponíveis: **Administrador** e **Voluntário**.
- Endpoints sensíveis (ex.: relatórios e cadastro de equipamentos) exigem perfil de administrador.
