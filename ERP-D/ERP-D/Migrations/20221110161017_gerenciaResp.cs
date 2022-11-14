using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_D.Migrations
{
    public partial class gerenciaResp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gerencias_Gerencias_GerenciaId",
                table: "Gerencias");

            migrationBuilder.RenameColumn(
                name: "GerenciaId",
                table: "Gerencias",
                newName: "DireccionId");

            migrationBuilder.RenameIndex(
                name: "IX_Gerencias_GerenciaId",
                table: "Gerencias",
                newName: "IX_Gerencias_DireccionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gerencias_Gerencias_DireccionId",
                table: "Gerencias",
                column: "DireccionId",
                principalTable: "Gerencias",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gerencias_Gerencias_DireccionId",
                table: "Gerencias");

            migrationBuilder.RenameColumn(
                name: "DireccionId",
                table: "Gerencias",
                newName: "GerenciaId");

            migrationBuilder.RenameIndex(
                name: "IX_Gerencias_DireccionId",
                table: "Gerencias",
                newName: "IX_Gerencias_GerenciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gerencias_Gerencias_GerenciaId",
                table: "Gerencias",
                column: "GerenciaId",
                principalTable: "Gerencias",
                principalColumn: "Id");
        }
    }
}
