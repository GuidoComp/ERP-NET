using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_D.Migrations
{
    public partial class imagenes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Gerencias_CentroDeCostoId",
                table: "Gerencias");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Imagenes",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_CentroDeCostoId",
                table: "Gerencias",
                column: "CentroDeCostoId",
                unique: true,
                filter: "[CentroDeCostoId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Gerencias_CentroDeCostoId",
                table: "Gerencias");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Imagenes");

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_CentroDeCostoId",
                table: "Gerencias",
                column: "CentroDeCostoId");
        }
    }
}
