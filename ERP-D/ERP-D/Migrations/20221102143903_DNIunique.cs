using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_D.Migrations
{
    public partial class DNIunique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Personas_DNI",
                table: "Personas",
                column: "DNI",
                unique: true,
                filter: "[DNI] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Personas_DNI",
                table: "Personas");
        }
    }
}
