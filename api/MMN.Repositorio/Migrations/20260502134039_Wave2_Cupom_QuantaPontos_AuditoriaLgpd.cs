using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MMN.Repositorio.Migrations
{
    /// <inheritdoc />
    public partial class Wave2_Cupom_QuantaPontos_AuditoriaLgpd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AssinaturaManual",
                table: "UsuarioProduto",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AgenteUltimoAcesso",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AsaasCustomerId",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataUltimoAcesso",
                table: "Usuario",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnderecoIPUltimoAcesso",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkAssistenteVirtual",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditoriaLgpd",
                columns: table => new
                {
                    IdAuditoriaLgpd = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuarioMaster = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUsuarioAlvo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Campo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IpOrigem = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataAcesso = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriaLgpd", x => x.IdAuditoriaLgpd);
                });

            migrationBuilder.CreateTable(
                name: "Cupom",
                columns: table => new
                {
                    IdCupom = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    Tipo = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ValidoDe = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidoAte = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MinimoPedido = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxUsosTotal = table.Column<int>(type: "int", nullable: true),
                    MaxUsosPorCliente = table.Column<int>(type: "int", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cupom", x => x.IdCupom);
                });

            migrationBuilder.CreateTable(
                name: "QuantaPontoLancamento",
                columns: table => new
                {
                    IdQuantaPontoLancamento = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Pontos = table.Column<int>(type: "int", nullable: false),
                    Origem = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IdReferencia = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DataLancamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantaPontoLancamento", x => x.IdQuantaPontoLancamento);
                });

            migrationBuilder.CreateTable(
                name: "CupomUso",
                columns: table => new
                {
                    IdCupomUso = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCupom = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdPedido = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ValorAplicado = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DataUso = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CupomUso", x => x.IdCupomUso);
                    table.ForeignKey(
                        name: "FK_CupomUso_Cupom_IdCupom",
                        column: x => x.IdCupom,
                        principalTable: "Cupom",
                        principalColumn: "IdCupom",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Cupom",
                columns: new[] { "IdCupom", "Ativo", "Codigo", "DataCadastro", "Descricao", "MaxUsosPorCliente", "MaxUsosTotal", "MinimoPedido", "Tipo", "ValidoAte", "ValidoDe", "Valor" },
                values: new object[,]
                {
                    { -2, true, "BEMVINDO", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "R$ 25 OFF para novos clientes", 1, null, 100m, "fixed", null, null, 25m },
                    { -1, true, "QUANTA10", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "10% de desconto na primeira compra", 1, null, 50m, "percent", null, null, 10m }
                });

            migrationBuilder.UpdateData(
                table: "ProvedorAutenticacao",
                keyColumn: "IdProvedorAutenticacao",
                keyValue: -1,
                columns: new[] { "EndpointCadastro", "EndpointLogin", "Login", "Senha", "UrlApi" },
                values: new object[] { "api/user/registrarGoogleCredential", "api/UsuarioLogin/autenticacaoGoogleCredential", "", "", "https://oauth2.googleapis.com/tokeninfo" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLgpd_IdUsuarioAlvo_DataAcesso",
                table: "AuditoriaLgpd",
                columns: new[] { "IdUsuarioAlvo", "DataAcesso" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriaLgpd_IdUsuarioMaster_DataAcesso",
                table: "AuditoriaLgpd",
                columns: new[] { "IdUsuarioMaster", "DataAcesso" });

            migrationBuilder.CreateIndex(
                name: "IX_Cupom_Codigo",
                table: "Cupom",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CupomUso_IdCupom_IdUsuario",
                table: "CupomUso",
                columns: new[] { "IdCupom", "IdUsuario" });

            migrationBuilder.CreateIndex(
                name: "IX_QuantaPontoLancamento_IdUsuario_Ativo",
                table: "QuantaPontoLancamento",
                columns: new[] { "IdUsuario", "Ativo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditoriaLgpd");

            migrationBuilder.DropTable(
                name: "CupomUso");

            migrationBuilder.DropTable(
                name: "QuantaPontoLancamento");

            migrationBuilder.DropTable(
                name: "Cupom");

            migrationBuilder.DropColumn(
                name: "AssinaturaManual",
                table: "UsuarioProduto");

            migrationBuilder.DropColumn(
                name: "AgenteUltimoAcesso",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "AsaasCustomerId",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "DataUltimoAcesso",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "EnderecoIPUltimoAcesso",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "LinkAssistenteVirtual",
                table: "Usuario");

            migrationBuilder.UpdateData(
                table: "ProvedorAutenticacao",
                keyColumn: "IdProvedorAutenticacao",
                keyValue: -1,
                columns: new[] { "EndpointCadastro", "EndpointLogin", "Login", "Senha", "UrlApi" },
                values: new object[] { "api/user/registrarGoogle", "api/UsuarioLogin/autenticacaoGoogle", "123493812146-gdjfhkeguuon50kjhhd6i3hgf4v172el.apps.googleusercontent.com", "69fWzUHFrkSE1HIfS8smM-Z-", "https://oauth2.googleapis.com/token" });
        }
    }
}
