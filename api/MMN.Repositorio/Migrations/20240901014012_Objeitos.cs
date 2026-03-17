using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMN.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class Objeitos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Objetivo",
                columns: table => new
                {
                    IdObjetivo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Grupo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrazoDe = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrazoAte = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objetivo", x => x.IdObjetivo);
                });

            migrationBuilder.CreateTable(
                name: "ObjetivoUsuario",
                columns: table => new
                {
                    IdObjetivoUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdObjetivo = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUsuarioFilho = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdPedido = table.Column<int>(type: "int", nullable: true),
                    DataRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjetivoUsuario", x => x.IdObjetivoUsuario);
                    table.ForeignKey(
                        name: "FK_ObjetivoUsuario_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObjetivoUsuario_Usuario_IdUsuarioFilho",
                        column: x => x.IdUsuarioFilho,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObjetivoUsuario_IdUsuario",
                table: "ObjetivoUsuario",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ObjetivoUsuario_IdUsuarioFilho",
                table: "ObjetivoUsuario",
                column: "IdUsuarioFilho");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Objetivo");

            migrationBuilder.DropTable(
                name: "ObjetivoUsuario");
        }
    }
}
