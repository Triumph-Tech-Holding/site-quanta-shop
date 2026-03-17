using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMN.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class QuantaAmizade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuantaAmizade",
                columns: table => new
                {
                    IdQuantaAmizade = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuarioPai = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUsuarioFilho = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ObjetivoAtingido = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantaAmizade", x => x.IdQuantaAmizade);
                    table.ForeignKey(
                        name: "FK_QuantaAmizade_Usuario_IdUsuarioFilho",
                        column: x => x.IdUsuarioFilho,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_QuantaAmizade_Usuario_IdUsuarioPai",
                        column: x => x.IdUsuarioPai,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "QuantaAmizadeHistorico",
                columns: table => new
                {
                    IdHistoricoCashbackUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdQuantaAmizade = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdPedido = table.Column<int>(type: "int", nullable: false),
                    DataCompra = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorCashback = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Finalizado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantaAmizadeHistorico", x => x.IdHistoricoCashbackUsuario);
                    table.ForeignKey(
                        name: "FK_QuantaAmizadeHistorico_QuantaAmizade_IdQuantaAmizade",
                        column: x => x.IdQuantaAmizade,
                        principalTable: "QuantaAmizade",
                        principalColumn: "IdQuantaAmizade",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_QuantaAmizadeHistorico_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuantaAmizade_IdUsuarioFilho",
                table: "QuantaAmizade",
                column: "IdUsuarioFilho");

            migrationBuilder.CreateIndex(
                name: "IX_QuantaAmizade_IdUsuarioPai",
                table: "QuantaAmizade",
                column: "IdUsuarioPai");

            migrationBuilder.CreateIndex(
                name: "IX_QuantaAmizadeHistorico_IdQuantaAmizade",
                table: "QuantaAmizadeHistorico",
                column: "IdQuantaAmizade");

            migrationBuilder.CreateIndex(
                name: "IX_QuantaAmizadeHistorico_IdUsuario",
                table: "QuantaAmizadeHistorico",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuantaAmizadeHistorico");

            migrationBuilder.DropTable(
                name: "QuantaAmizade");
        }
    }
}
