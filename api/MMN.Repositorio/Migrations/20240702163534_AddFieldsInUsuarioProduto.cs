using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMN.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsInUsuarioProduto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssinaturaAte",
                table: "UsuarioProduto",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssinaturaDe",
                table: "UsuarioProduto",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssinaturaProximaCobranca",
                table: "UsuarioProduto",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAssinatura",
                table: "UsuarioProduto",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssinaturaAte",
                table: "UsuarioProduto");

            migrationBuilder.DropColumn(
                name: "AssinaturaDe",
                table: "UsuarioProduto");

            migrationBuilder.DropColumn(
                name: "AssinaturaProximaCobranca",
                table: "UsuarioProduto");

            migrationBuilder.DropColumn(
                name: "DataAssinatura",
                table: "UsuarioProduto");
        }
    }
}
