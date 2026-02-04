# Igreja Social - Dashboard

Painel administrativo para gestão de famílias atendidas, empréstimos de equipamentos e entregas de cestas básicas, com módulos para doações avulsas, visitas e pessoas em situação de rua.

## Visão geral
- **Backend** em ASP.NET Core 8 com EF Core, Identity e documentação via Swagger.
- **Frontend** em Blazor WebAssembly com MudBlazor, servido pela API no mesmo host.
- **Banco de dados** SQL Server (configurado em `DefaultConnection`).
- **Integrações**: ViaCEP para busca de CEP e QuestPDF para relatórios em PDF.

## Estrutura do repositório
```
.
├── IgrejaSocial.slnx
├── README.md
├── scripts/
│   └── backup-db.sh
├── src/
│   ├── IgrejaSocial.API            # API + Identity + Swagger + hosting do Blazor WASM
│   ├── IgrejaSocial.Application    # Serviços de aplicação, DTOs e mapeamentos
│   ├── IgrejaSocial.Domain         # Entidades, enums, regras de domínio
│   ├── IgrejaSocial.Infrastructure # EF Core, repositórios, integrações externas
│   ├── IgrejaSocial.Web            # Blazor WebAssembly + MudBlazor
│   └── IgrejaSocial.db             # Banco SQLite local (exemplo)
└── tests/
    └── IgrejaSocial.IntegrationTests
```

## Projetos e responsabilidades
- **IgrejaSocial.API**: APIs REST, autenticação, autorização, relatórios e hosting do front-end.
- **IgrejaSocial.Application**: DTOs, serviços e mapeamentos (AutoMapper).
- **IgrejaSocial.Domain**: entidades (Família, Equipamento, Registro de Atendimento, etc.), enums e validações.
- **IgrejaSocial.Infrastructure**: EF Core (DbContext e migrations), repositórios e integrações (ViaCEP).
- **IgrejaSocial.Web**: Blazor WASM com páginas e serviços de UI.

## Funcionalidades
- **Famílias e membros**: cadastro e acompanhamento.
- **Equipamentos e empréstimos**: controle patrimonial, estado e devoluções.
- **Cestas básicas**: registro de entregas.
- **Doações avulsas**: registro de doações pontuais.
- **Pessoas em situação de rua**: acompanhamento e atendimentos.
- **Visitas**: histórico e registros de visita.
- **Dashboard**: indicadores e relatórios.
- **Relatórios mensais**:
  - Exportação de atendimentos do mês (CSV).
  - PDF de KPIs (QuestPDF), incluindo giro de estoque e tempo médio de empréstimo.

## Endpoints principais (API)
- **Auth**: login e identidade.
- **CEP**: consulta via ViaCEP.
- **Cestas**: entrega e histórico.
- **Dashboard**: indicadores.
- **Doações avulsas**: registro e listagem.
- **Empréstimos**: cadastro e devolução de equipamentos.
- **Equipamentos**: controle de patrimônio.
- **Famílias**: cadastro e acompanhamento.
- **Pessoas em situação de rua**: registros e atendimentos.
- **Relatórios**: exportações e PDFs.
- **Visitas**: histórico de visitas.

## Páginas principais (Blazor)
- **Home/Dashboard**
- **Famílias**
- **Equipamentos**
- **Empréstimos**
- **Cestas**
- **Doações avulsas**
- **Pessoas em situação de rua**
- **Visitas**
- **Mapa**
- **Autenticação**

## Requisitos
- .NET 8 SDK
- SQL Server (ou ajuste a connection string)

## Configuração
As principais configurações ficam em `src/IgrejaSocial.API/appsettings.json` e podem ser sobrescritas por variáveis de ambiente:

- **ConnectionStrings__DefaultConnection**: string de conexão do SQL Server.
- **CepService__BaseUrl**: base URL do ViaCEP (padrão: `https://viacep.com.br/ws/`).
- **AdminSeed__Email** e **AdminSeed__Password**: cria usuário administrador no primeiro start.

> O seeding cria as roles **Administrador** e **Voluntário**.

Exemplo de `.env` (copie de `.env.example`):
```env
ADMINSEED__EMAIL=admin@local.test
ADMINSEED__PASSWORD=ChangeMe123!
```

## Como rodar o projeto (API + UI)
1. Configure a connection string usando variáveis de ambiente ou user-secrets.
2. Execute a API (ela hospeda o Blazor WebAssembly):

```bash
dotnet run --project src/IgrejaSocial.API/IgrejaSocial.API.csproj
```

3. Acesse:
   - UI: `https://localhost:5001`
   - Swagger: `https://localhost:5001/swagger`

## Testes
Testes de integração cobrem fluxos como empréstimos de equipamentos e entrega de cestas:

```bash
dotnet test
```

## Manutenção: adicionando novos tipos de equipamentos
1. Atualize o enum `TipoEquipamento` em `src/IgrejaSocial.Domain/Enums/EquipamentoEnums.cs`.
2. Caso use dados seed, acrescente o novo tipo em `IgrejaSocialDbContext` (se aplicável).
3. A UI lista os tipos automaticamente via `Enum.GetValues<TipoEquipamento>()`.

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
