using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IgrejaSocial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoInicialFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoPatrimonio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDisponivel = table.Column<bool>(type: "bit", nullable: false),
                    DataAquisicao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorEstimado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ObservacoesInternas = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Familias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeResponsavel = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CpfResponsavel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RgResponsavel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DocumentacaoApresentada = table.Column<bool>(type: "bit", nullable: false),
                    Residencia = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataNascimentoResponsavel = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Endereco = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TelefoneContato = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    RendaFamiliarTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Membros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Parentesco = table.Column<int>(type: "int", nullable: false),
                    NivelEscolar = table.Column<int>(type: "int", nullable: false),
                    SituacaoTrabalho = table.Column<int>(type: "int", nullable: false),
                    RendaIndividual = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    PossuiDeficiencia = table.Column<bool>(type: "bit", nullable: false),
                    FamiliaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Membros_Familias_FamiliaId",
                        column: x => x.FamiliaId,
                        principalTable: "Familias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosAtendimento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FamiliaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EquipamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TipoAtendimento = table.Column<int>(type: "int", nullable: false),
                    DataEntrega = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataEmprestimo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataPrevistaDevolucao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataDevolucaoReal = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosAtendimento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosAtendimento_Equipamentos_EquipamentoId",
                        column: x => x.EquipamentoId,
                        principalTable: "Equipamentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistrosAtendimento_Familias_FamiliaId",
                        column: x => x.FamiliaId,
                        principalTable: "Familias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Equipamentos_CodigoPatrimonio",
                table: "Equipamentos",
                column: "CodigoPatrimonio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Familias_CpfResponsavel",
                table: "Familias",
                column: "CpfResponsavel",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Membros_FamiliaId",
                table: "Membros",
                column: "FamiliaId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosAtendimento_EquipamentoId",
                table: "RegistrosAtendimento",
                column: "EquipamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosAtendimento_FamiliaId",
                table: "RegistrosAtendimento",
                column: "FamiliaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrosAtendimento");

            migrationBuilder.DropTable(
                name: "Equipamentos");

            migrationBuilder.DropTable(
                name: "Membros");

            migrationBuilder.DropTable(
                name: "Familias");
        }
    }
}
