﻿// <auto-generated />
using System;
using ERP_D.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ERP_D.Migrations
{
    [DbContext(typeof(ErpContext))]
    [Migration("20221021212744_Inicial")]
    partial class Inicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ERP_D.Models.CentroDeCosto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("MontoMaximo")
                        .HasColumnType("float");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.HasKey("Id");

                    b.ToTable("CentrosDeCosto");
                });

            modelBuilder.Entity("ERP_D.Models.Empresa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GerenciaGeneralId")
                        .HasColumnType("int");

                    b.Property<string>("Logo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rubro")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GerenciaGeneralId");

                    b.ToTable("Empresas");
                });

            modelBuilder.Entity("ERP_D.Models.Gasto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CentroDeCostoId")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<int>("EmpleadoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<double>("Monto")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CentroDeCostoId");

                    b.HasIndex("EmpleadoId");

                    b.ToTable("Gastos");
                });

            modelBuilder.Entity("ERP_D.Models.Gerencia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CentroDeCostoId")
                        .HasColumnType("int");

                    b.Property<int>("EmpresaId")
                        .HasColumnType("int");

                    b.Property<bool>("EsGerenciaGeneral")
                        .HasColumnType("bit");

                    b.Property<int?>("GerenciaId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<int?>("ResponsableId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CentroDeCostoId");

                    b.HasIndex("EmpresaId");

                    b.HasIndex("GerenciaId");

                    b.HasIndex("ResponsableId");

                    b.ToTable("Gerencias");
                });

            modelBuilder.Entity("ERP_D.Models.Imagen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Imagenes");
                });

            modelBuilder.Entity("ERP_D.Models.Persona", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<int>("DNI")
                        .HasColumnType("int");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FechaAlta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Personas");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Persona");
                });

            modelBuilder.Entity("ERP_D.Models.Posicion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Descripcion")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("GerenciaId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<int?>("ResponsableId")
                        .HasColumnType("int");

                    b.Property<decimal>("Sueldo")
                        .HasPrecision(38, 18)
                        .HasColumnType("decimal(38,18)");

                    b.HasKey("Id");

                    b.HasIndex("GerenciaId");

                    b.HasIndex("ResponsableId");

                    b.ToTable("Posiciones");
                });

            modelBuilder.Entity("ERP_D.Models.Telefono", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonaId")
                        .HasColumnType("int");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PersonaId");

                    b.ToTable("Telefonos");
                });

            modelBuilder.Entity("ERP_D.Models.Empleado", b =>
                {
                    b.HasBaseType("ERP_D.Models.Persona");

                    b.Property<bool>("EmpleadoActivo")
                        .HasColumnType("bit");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Legajo")
                        .HasColumnType("int");

                    b.Property<string>("ObraSocial")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PosicionId")
                        .HasColumnType("int");

                    b.HasIndex("PosicionId")
                        .IsUnique()
                        .HasFilter("[PosicionId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("Empleado");
                });

            modelBuilder.Entity("ERP_D.Models.Empresa", b =>
                {
                    b.HasOne("ERP_D.Models.Gerencia", "GerenciaGeneral")
                        .WithMany()
                        .HasForeignKey("GerenciaGeneralId");

                    b.Navigation("GerenciaGeneral");
                });

            modelBuilder.Entity("ERP_D.Models.Gasto", b =>
                {
                    b.HasOne("ERP_D.Models.CentroDeCosto", "CentroDeCosto")
                        .WithMany("Gastos")
                        .HasForeignKey("CentroDeCostoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ERP_D.Models.Empleado", "Empleado")
                        .WithMany("Gastos")
                        .HasForeignKey("EmpleadoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CentroDeCosto");

                    b.Navigation("Empleado");
                });

            modelBuilder.Entity("ERP_D.Models.Gerencia", b =>
                {
                    b.HasOne("ERP_D.Models.CentroDeCosto", "CentroDeCosto")
                        .WithMany()
                        .HasForeignKey("CentroDeCostoId");

                    b.HasOne("ERP_D.Models.Empresa", "Empresa")
                        .WithMany("Gerencias")
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ERP_D.Models.Gerencia", "Direccion")
                        .WithMany("SubGerencias")
                        .HasForeignKey("GerenciaId");

                    b.HasOne("ERP_D.Models.Posicion", "Responsable")
                        .WithMany()
                        .HasForeignKey("ResponsableId");

                    b.Navigation("CentroDeCosto");

                    b.Navigation("Direccion");

                    b.Navigation("Empresa");

                    b.Navigation("Responsable");
                });

            modelBuilder.Entity("ERP_D.Models.Posicion", b =>
                {
                    b.HasOne("ERP_D.Models.Gerencia", "Gerencia")
                        .WithMany("Posiciones")
                        .HasForeignKey("GerenciaId");

                    b.HasOne("ERP_D.Models.Posicion", "Responsable")
                        .WithMany("Subordinadas")
                        .HasForeignKey("ResponsableId");

                    b.Navigation("Gerencia");

                    b.Navigation("Responsable");
                });

            modelBuilder.Entity("ERP_D.Models.Telefono", b =>
                {
                    b.HasOne("ERP_D.Models.Persona", "Persona")
                        .WithMany("Telefonos")
                        .HasForeignKey("PersonaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Persona");
                });

            modelBuilder.Entity("ERP_D.Models.Empleado", b =>
                {
                    b.HasOne("ERP_D.Models.Posicion", "Posicion")
                        .WithOne("Empleado")
                        .HasForeignKey("ERP_D.Models.Empleado", "PosicionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Posicion");
                });

            modelBuilder.Entity("ERP_D.Models.CentroDeCosto", b =>
                {
                    b.Navigation("Gastos");
                });

            modelBuilder.Entity("ERP_D.Models.Empresa", b =>
                {
                    b.Navigation("Gerencias");
                });

            modelBuilder.Entity("ERP_D.Models.Gerencia", b =>
                {
                    b.Navigation("Posiciones");

                    b.Navigation("SubGerencias");
                });

            modelBuilder.Entity("ERP_D.Models.Persona", b =>
                {
                    b.Navigation("Telefonos");
                });

            modelBuilder.Entity("ERP_D.Models.Posicion", b =>
                {
                    b.Navigation("Empleado");

                    b.Navigation("Subordinadas");
                });

            modelBuilder.Entity("ERP_D.Models.Empleado", b =>
                {
                    b.Navigation("Gastos");
                });
#pragma warning restore 612, 618
        }
    }
}