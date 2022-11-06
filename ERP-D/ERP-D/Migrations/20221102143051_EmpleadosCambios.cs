using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_D.Migrations
{
    public partial class EmpleadosCambios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "Legajo",
                startValue: 100L);

            migrationBuilder.AlterColumn<int>(
                name: "Legajo",
                table: "Personas",
                type: "int",
                nullable: true,
                defaultValueSql: "NEXT VALUE FOR Legajo",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "Legajo");

            migrationBuilder.AlterColumn<int>(
                name: "Legajo",
                table: "Personas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "NEXT VALUE FOR Legajo");
        }
    }
}
