using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMN.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCarousselFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarrosselAnunciante");

            migrationBuilder.RenameColumn(
                name: "Titulo",
                table: "Carrossel",
                newName: "Texto3");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Carrossel",
                newName: "Texto2");

            migrationBuilder.RenameColumn(
                name: "Descricao",
                table: "Carrossel",
                newName: "Texto1");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Carrossel",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "Carrossel");

            migrationBuilder.RenameColumn(
                name: "Texto3",
                table: "Carrossel",
                newName: "Titulo");

            migrationBuilder.RenameColumn(
                name: "Texto2",
                table: "Carrossel",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "Texto1",
                table: "Carrossel",
                newName: "Descricao");

            migrationBuilder.CreateTable(
                name: "CarrosselAnunciante",
                columns: table => new
                {
                    IdCarrosselAnunciante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAnunciante = table.Column<int>(type: "int", nullable: false),
                    IdCarrossel = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataSaida = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ordem = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrosselAnunciante", x => x.IdCarrosselAnunciante);
                    table.ForeignKey(
                        name: "FK_CarrosselAnunciante_Anunciante_IdAnunciante",
                        column: x => x.IdAnunciante,
                        principalTable: "Anunciante",
                        principalColumn: "IdAnunciante",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarrosselAnunciante_Carrossel_IdCarrossel",
                        column: x => x.IdCarrossel,
                        principalTable: "Carrossel",
                        principalColumn: "IdCarrossel",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarrosselAnunciante_IdAnunciante",
                table: "CarrosselAnunciante",
                column: "IdAnunciante");

            migrationBuilder.CreateIndex(
                name: "IX_CarrosselAnunciante_IdCarrossel",
                table: "CarrosselAnunciante",
                column: "IdCarrossel");
        }
    }
}
