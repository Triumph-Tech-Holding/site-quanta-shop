using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMN.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToCredenciamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
             name: "AceitaPgtoComSaldo",
             table: "Credenciamento",
             type: "bit",
             nullable: false,
             defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RazaoSocial",
                table: "Credenciamento",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ScanGo",
                table: "Credenciamento",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssinaturaHabilitada",
                table: "UsuarioProduto");

            migrationBuilder.DropColumn(
                name: "CodigoReferenciaAssinatura",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "AceitaPgtoComSaldo",
                table: "Credenciamento");

            migrationBuilder.DropColumn(
                name: "RazaoSocial",
                table: "Credenciamento");

            migrationBuilder.DropColumn(
                name: "ScanGo",
                table: "Credenciamento");
        }
    }
}
