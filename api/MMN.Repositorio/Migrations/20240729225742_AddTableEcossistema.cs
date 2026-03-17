using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMN.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class AddTableEcossistema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EcossistemaIdEcossistema",
                table: "Credenciamento",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdEcossistema",
                table: "Credenciamento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Ecossistema",
                columns: table => new
                {
                    IdEcossistema = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Regiao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    IdUsuarioGerente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ecossistema", x => x.IdEcossistema);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credenciamento_EcossistemaIdEcossistema",
                table: "Credenciamento",
                column: "EcossistemaIdEcossistema");

            migrationBuilder.AddForeignKey(
                name: "FK_Credenciamento_Ecossistema_EcossistemaIdEcossistema",
                table: "Credenciamento",
                column: "EcossistemaIdEcossistema",
                principalTable: "Ecossistema",
                principalColumn: "IdEcossistema");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credenciamento_Ecossistema_EcossistemaIdEcossistema",
                table: "Credenciamento");

            migrationBuilder.DropTable(
                name: "Ecossistema");

            migrationBuilder.DropIndex(
                name: "IX_Credenciamento_EcossistemaIdEcossistema",
                table: "Credenciamento");

            migrationBuilder.DropColumn(
                name: "EcossistemaIdEcossistema",
                table: "Credenciamento");

            migrationBuilder.DropColumn(
                name: "IdEcossistema",
                table: "Credenciamento");
        }
    }
}
