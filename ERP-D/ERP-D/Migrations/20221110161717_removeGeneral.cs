using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_D.Migrations
{
    public partial class removeGeneral : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Gerencias_EsGerenciaGeneral",
                table: "Gerencias");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_EsGerenciaGeneral",
                table: "Gerencias",
                column: "EsGerenciaGeneral",
                unique: true);
        }
    }
}
