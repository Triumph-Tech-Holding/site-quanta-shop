using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MMN.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlChaveDeAcessoNF",
                table: "CuponCashback",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ancora",
                table: "Anunciante",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "UrlChaveDeAcessoNF",
                table: "CuponCashback");

            migrationBuilder.DropColumn(
                name: "Ancora",
                table: "Anunciante");
        }
    }
}
