using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMN.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class ajusteFKCredenciado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.AlterColumn<int>(
                name: "IdEcossistema",
                table: "Credenciamento",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Credenciamento_IdEcossistema",
                table: "Credenciamento",
                column: "IdEcossistema");

            migrationBuilder.AddForeignKey(
                name: "FK_Credenciamento_Ecossistema",
                table: "Credenciamento",
                column: "IdEcossistema",
                principalTable: "Ecossistema",
                principalColumn: "IdEcossistema");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credenciamento_Ecossistema_IdEcossistema",
                table: "Credenciamento");

            migrationBuilder.DropIndex(
                name: "IX_Credenciamento_IdEcossistema",
                table: "Credenciamento");

            migrationBuilder.AlterColumn<int>(
                name: "IdEcossistema",
                table: "Credenciamento",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EcossistemaIdEcossistema",
                table: "Credenciamento",
                type: "int",
                nullable: true);

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
    }
}
