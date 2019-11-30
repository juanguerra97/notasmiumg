﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NotasMiUMGWebApp.Data;

namespace NotasMiUMGWebApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.DeviceFlowCodes", b =>
                {
                    b.Property<string>("UserCode")
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasMaxLength(50000);

                    b.Property<string>("DeviceCode")
                        .IsRequired()
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("Expiration")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SubjectId")
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.HasKey("UserCode");

                    b.HasIndex("DeviceCode")
                        .IsUnique();

                    b.HasIndex("Expiration");

                    b.ToTable("DeviceCodes");
                });

            modelBuilder.Entity("IdentityServer4.EntityFramework.Entities.PersistedGrant", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4")
                        .HasMaxLength(50000);

                    b.Property<DateTime?>("Expiration")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("SubjectId")
                        .HasColumnType("varchar(200) CHARACTER SET utf8mb4")
                        .HasMaxLength(200);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("varchar(50) CHARACTER SET utf8mb4")
                        .HasMaxLength(50);

                    b.HasKey("Key");

                    b.HasIndex("Expiration");

                    b.HasIndex("SubjectId", "ClientId", "Type");

                    b.ToTable("PersistedGrants");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("varchar(256) CHARACTER SET utf8mb4")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(128) CHARACTER SET utf8mb4")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("jguerra.notasmiumg.Carrera", b =>
                {
                    b.Property<uint>("CodigoCarrera")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<string>("NombreCarrera")
                        .IsRequired()
                        .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                        .HasMaxLength(64);

                    b.HasKey("CodigoCarrera");

                    b.HasAlternateKey("NombreCarrera");

                    b.ToTable("Carreras");
                });

            modelBuilder.Entity("jguerra.notasmiumg.Curso", b =>
                {
                    b.Property<uint>("CodigoCarrera")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("CodigoCurso")
                        .HasColumnType("int unsigned");

                    b.Property<string>("NombreCurso")
                        .IsRequired()
                        .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                        .HasMaxLength(64);

                    b.HasKey("CodigoCarrera", "CodigoCurso");

                    b.HasAlternateKey("CodigoCarrera", "NombreCurso");

                    b.ToTable("Cursos");
                });

            modelBuilder.Entity("jguerra.notasmiumg.Estudiante", b =>
                {
                    b.Property<uint>("EstudianteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int unsigned");

                    b.Property<uint>("AnoInicio")
                        .HasColumnType("int unsigned");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                        .HasMaxLength(64);

                    b.Property<string>("Carne")
                        .IsRequired()
                        .HasColumnType("varchar(16) CHARACTER SET utf8mb4")
                        .HasMaxLength(16);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("varchar(64) CHARACTER SET utf8mb4")
                        .HasMaxLength(64);

                    b.Property<uint?>("PensumAnoPensum")
                        .HasColumnType("int unsigned");

                    b.Property<uint?>("PensumCodigoCarrera")
                        .HasColumnType("int unsigned");

                    b.Property<string>("UsuarioEstudianteId")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("EstudianteId");

                    b.HasAlternateKey("Carne");

                    b.HasIndex("UsuarioEstudianteId");

                    b.HasIndex("PensumCodigoCarrera", "PensumAnoPensum");

                    b.ToTable("Estudiantes");
                });

            modelBuilder.Entity("jguerra.notasmiumg.Nota", b =>
                {
                    b.Property<uint>("EstudianteId")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("CodigoCarrera")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("AnoPensum")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("CodigoCurso")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("Ano")
                        .HasColumnType("int unsigned");

                    b.Property<byte>("Actividades")
                        .HasColumnType("tinyint unsigned");

                    b.Property<bool>("Aprobado")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tinyint(1)")
                        .HasComputedColumnSql("NotaFinal >= 61");

                    b.Property<byte>("ExamenFinal")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte>("NotaFinal")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tinyint unsigned")
                        .HasComputedColumnSql("Zona + ExamenFinal");

                    b.Property<byte>("PrimerParcial")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte>("SegundoParcial")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte>("Zona")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tinyint unsigned")
                        .HasComputedColumnSql("PrimerParcial + SegundoParcial + Actividades");

                    b.HasKey("EstudianteId", "CodigoCarrera", "AnoPensum", "CodigoCurso", "Ano");

                    b.HasIndex("CodigoCarrera", "AnoPensum", "CodigoCurso");

                    b.ToTable("Notas");
                });

            modelBuilder.Entity("jguerra.notasmiumg.Pensum", b =>
                {
                    b.Property<uint>("CodigoCarrera")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("AnoPensum")
                        .HasColumnType("int unsigned");

                    b.HasKey("CodigoCarrera", "AnoPensum");

                    b.ToTable("Pensums");
                });

            modelBuilder.Entity("jguerra.notasmiumg.PensumCurso", b =>
                {
                    b.Property<uint>("CodigoCarrera")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("AnoPensum")
                        .HasColumnType("int unsigned");

                    b.Property<uint>("CodigoCurso")
                        .HasColumnType("int unsigned");

                    b.Property<byte>("Ciclo")
                        .HasColumnType("tinyint unsigned");

                    b.Property<byte>("Creditos")
                        .HasColumnType("tinyint unsigned");

                    b.HasKey("CodigoCarrera", "AnoPensum", "CodigoCurso");

                    b.HasIndex("CodigoCarrera", "CodigoCurso");

                    b.ToTable("PensumCursos");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("jguerra.notasmiumg.Curso", b =>
                {
                    b.HasOne("jguerra.notasmiumg.Carrera", "Carrera")
                        .WithMany("Cursos")
                        .HasForeignKey("CodigoCarrera")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("jguerra.notasmiumg.Estudiante", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "UsuarioEstudiante")
                        .WithMany()
                        .HasForeignKey("UsuarioEstudianteId");

                    b.HasOne("jguerra.notasmiumg.Pensum", "Pensum")
                        .WithMany("Estudiantes")
                        .HasForeignKey("PensumCodigoCarrera", "PensumAnoPensum")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("jguerra.notasmiumg.Nota", b =>
                {
                    b.HasOne("jguerra.notasmiumg.Estudiante", "Estudiante")
                        .WithMany("Notas")
                        .HasForeignKey("EstudianteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("jguerra.notasmiumg.PensumCurso", "PensumCurso")
                        .WithMany("Notas")
                        .HasForeignKey("CodigoCarrera", "AnoPensum", "CodigoCurso")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("jguerra.notasmiumg.Pensum", b =>
                {
                    b.HasOne("jguerra.notasmiumg.Carrera", "Carrera")
                        .WithMany("Pensums")
                        .HasForeignKey("CodigoCarrera")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("jguerra.notasmiumg.PensumCurso", b =>
                {
                    b.HasOne("jguerra.notasmiumg.Pensum", "Pensum")
                        .WithMany("PensumCursos")
                        .HasForeignKey("CodigoCarrera", "AnoPensum")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("jguerra.notasmiumg.Curso", "Curso")
                        .WithMany("PensumCursos")
                        .HasForeignKey("CodigoCarrera", "CodigoCurso")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
