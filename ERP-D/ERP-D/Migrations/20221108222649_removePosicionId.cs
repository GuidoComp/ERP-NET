using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_D.Migrations
{
    public partial class removePosicionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personas_Posiciones_PosicionId",
                table: "Personas");

            migrationBuilder.AddForeignKey(
                name: "FK_Personas_Posiciones_PosicionId",
                table: "Personas",
                column: "PosicionId",
                principalTable: "Posiciones",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personas_Posiciones_PosicionId",
                table: "Personas");

            migrationBuilder.AddForeignKey(
                name: "FK_Personas_Posiciones_PosicionId",
                table: "Personas",
                column: "PosicionId",
                principalTable: "Posiciones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
