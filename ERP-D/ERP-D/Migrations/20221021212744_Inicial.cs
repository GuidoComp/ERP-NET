using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP_D.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CentrosDeCosto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    MontoMaximo = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentrosDeCosto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rubro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GerenciaGeneralId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gastos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Monto = table.Column<double>(type: "float", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmpleadoId = table.Column<int>(type: "int", nullable: false),
                    CentroDeCostoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gastos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gastos_CentrosDeCosto_CentroDeCostoId",
                        column: x => x.CentroDeCostoId,
                        principalTable: "CentrosDeCosto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gerencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    EsGerenciaGeneral = table.Column<bool>(type: "bit", nullable: false),
                    GerenciaId = table.Column<int>(type: "int", nullable: true),
                    ResponsableId = table.Column<int>(type: "int", nullable: true),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    CentroDeCostoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gerencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gerencias_CentrosDeCosto_CentroDeCostoId",
                        column: x => x.CentroDeCostoId,
                        principalTable: "CentrosDeCosto",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Gerencias_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gerencias_Gerencias_GerenciaId",
                        column: x => x.GerenciaId,
                        principalTable: "Gerencias",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Posiciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Sueldo = table.Column<decimal>(type: "decimal(38,18)", precision: 38, scale: 18, nullable: false),
                    ResponsableId = table.Column<int>(type: "int", nullable: true),
                    GerenciaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posiciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posiciones_Gerencias_GerenciaId",
                        column: x => x.GerenciaId,
                        principalTable: "Gerencias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Posiciones_Posiciones_ResponsableId",
                        column: x => x.ResponsableId,
                        principalTable: "Posiciones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DNI = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getdate()"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Legajo = table.Column<int>(type: "int", nullable: true),
                    ObraSocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpleadoActivo = table.Column<bool>(type: "bit", nullable: true),
                    PosicionId = table.Column<int>(type: "int", nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personas_Posiciones_PosicionId",
                        column: x => x.PosicionId,
                        principalTable: "Posiciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Telefonos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersonaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telefonos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telefonos_Personas_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Personas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_GerenciaGeneralId",
                table: "Empresas",
                column: "GerenciaGeneralId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_CentroDeCostoId",
                table: "Gastos",
                column: "CentroDeCostoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_EmpleadoId",
                table: "Gastos",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_CentroDeCostoId",
                table: "Gerencias",
                column: "CentroDeCostoId");

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_EmpresaId",
                table: "Gerencias",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_GerenciaId",
                table: "Gerencias",
                column: "GerenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Gerencias_ResponsableId",
                table: "Gerencias",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_PosicionId",
                table: "Personas",
                column: "PosicionId",
                unique: true,
                filter: "[PosicionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Posiciones_GerenciaId",
                table: "Posiciones",
                column: "GerenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Posiciones_ResponsableId",
                table: "Posiciones",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Telefonos_PersonaId",
                table: "Telefonos",
                column: "PersonaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_Gerencias_GerenciaGeneralId",
                table: "Empresas",
                column: "GerenciaGeneralId",
                principalTable: "Gerencias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Personas_EmpleadoId",
                table: "Gastos",
                column: "EmpleadoId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gerencias_Posiciones_ResponsableId",
                table: "Gerencias",
                column: "ResponsableId",
                principalTable: "Posiciones",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Gerencias_GerenciaGeneralId",
                table: "Empresas");

            migrationBuilder.DropForeignKey(
                name: "FK_Posiciones_Gerencias_GerenciaId",
                table: "Posiciones");

            migrationBuilder.DropTable(
                name: "Gastos");

            migrationBuilder.DropTable(
                name: "Imagenes");

            migrationBuilder.DropTable(
                name: "Telefonos");

            migrationBuilder.DropTable(
                name: "Personas");

            migrationBuilder.DropTable(
                name: "Gerencias");

            migrationBuilder.DropTable(
                name: "CentrosDeCosto");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "Posiciones");
        }
    }
}
