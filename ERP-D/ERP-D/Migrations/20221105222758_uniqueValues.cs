using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_D.Migrations
{
    public partial class uniqueValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Imagenes");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Empresas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Posiciones_Nombre",
                table: "Posiciones",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_EsGerenciaGeneral",
                table: "Gerencias",
                column: "EsGerenciaGeneral",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_Nombre",
                table: "Gerencias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Nombre",
                table: "Empresas",
                column: "Nombre",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posiciones_Nombre",
                table: "Posiciones");

            migrationBuilder.DropIndex(
                name: "IX_Gerencias_EsGerenciaGeneral",
                table: "Gerencias");

            migrationBuilder.DropIndex(
                name: "IX_Gerencias_Nombre",
                table: "Gerencias");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_Nombre",
                table: "Empresas");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Imagenes",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Empresas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
