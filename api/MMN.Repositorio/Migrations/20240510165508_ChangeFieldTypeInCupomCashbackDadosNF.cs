using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMN.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFieldTypeInCupomCashbackDadosNF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "QuantidadeProduto",
                table: "CupomCashbackItemNF",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "QtdTotalDeItens",
                table: "CupomCashbackDadosNF",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
