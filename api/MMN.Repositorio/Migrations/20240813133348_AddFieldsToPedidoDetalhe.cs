using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMN.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToPedidoDetalhe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssinaturaAte",
                table: "PedidoDetalhe",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssinaturaDe",
                table: "PedidoDetalhe",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssinaturaProximaCobranca",
                table: "PedidoDetalhe",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAssinatura",
                table: "PedidoDetalhe",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssinaturaAte",
                table: "PedidoDetalhe");

            migrationBuilder.DropColumn(
                name: "AssinaturaDe",
                table: "PedidoDetalhe");

            migrationBuilder.DropColumn(
                name: "AssinaturaProximaCobranca",
                table: "PedidoDetalhe");

            migrationBuilder.DropColumn(
                name: "DataAssinatura",
                table: "PedidoDetalhe");
        }
    }
}
