using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_D.Migrations
{
    public partial class removeGerenciaGen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Gerencias_GerenciaGeneralId",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_GerenciaGeneralId",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "GerenciaGeneralId",
                table: "Empresas");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GerenciaGeneralId",
                table: "Empresas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_GerenciaGeneralId",
                table: "Empresas",
                column: "GerenciaGeneralId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_Gerencias_GerenciaGeneralId",
                table: "Empresas",
                column: "GerenciaGeneralId",
                principalTable: "Gerencias",
                principalColumn: "Id");
        }
    }
}
