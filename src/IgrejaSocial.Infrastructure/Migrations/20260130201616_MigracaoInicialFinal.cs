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
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CodigoPatrimonio = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsDisponivel = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataAquisicao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ValorEstimado = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    ObservacoesInternas = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Familias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    NomeResponsavel = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    CpfResponsavel = table.Column<string>(type: "TEXT", nullable: false),
                    Residencia = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Endereco = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    TelefoneContato = table.Column<string>(type: "TEXT", nullable: false),
                    RendaFamiliarTotal = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Membros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Parentesco = table.Column<int>(type: "INTEGER", nullable: false),
                    NivelEscolar = table.Column<int>(type: "INTEGER", nullable: false),
                    SituacaoTrabalho = table.Column<int>(type: "INTEGER", nullable: false),
                    Cpf = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false),
                    PossuiDeficiencia = table.Column<bool>(type: "INTEGER", nullable: false),
                    FamiliaId = table.Column<Guid>(type: "TEXT", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equipamentos");

            migrationBuilder.DropTable(
                name: "Membros");

            migrationBuilder.DropTable(
                name: "Familias");
        }
    }
}
