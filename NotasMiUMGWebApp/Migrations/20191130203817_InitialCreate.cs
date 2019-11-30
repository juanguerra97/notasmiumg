using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NotasMiUMGWebApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Carreras",
                columns: table => new
                {
                    CodigoCarrera = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreCarrera = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreras", x => x.CodigoCarrera);
                    table.UniqueConstraint("AK_Carreras_NombreCarrera", x => x.NombreCarrera);
                });

            migrationBuilder.CreateTable(
                name: "DeviceCodes",
                columns: table => new
                {
                    UserCode = table.Column<string>(maxLength: 200, nullable: false),
                    DeviceCode = table.Column<string>(maxLength: 200, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceCodes", x => x.UserCode);
                });

            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(maxLength: 200, nullable: false),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cursos",
                columns: table => new
                {
                    CodigoCurso = table.Column<uint>(nullable: false),
                    CodigoCarrera = table.Column<uint>(nullable: false),
                    NombreCurso = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursos", x => new { x.CodigoCarrera, x.CodigoCurso });
                    table.UniqueConstraint("AK_Cursos_CodigoCarrera_NombreCurso", x => new { x.CodigoCarrera, x.NombreCurso });
                    table.ForeignKey(
                        name: "FK_Cursos_Carreras_CodigoCarrera",
                        column: x => x.CodigoCarrera,
                        principalTable: "Carreras",
                        principalColumn: "CodigoCarrera",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pensums",
                columns: table => new
                {
                    AnoPensum = table.Column<uint>(nullable: false),
                    CodigoCarrera = table.Column<uint>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pensums", x => new { x.CodigoCarrera, x.AnoPensum });
                    table.ForeignKey(
                        name: "FK_Pensums_Carreras_CodigoCarrera",
                        column: x => x.CodigoCarrera,
                        principalTable: "Carreras",
                        principalColumn: "CodigoCarrera",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    EstudianteId = table.Column<uint>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Carne = table.Column<string>(maxLength: 16, nullable: false),
                    Nombre = table.Column<string>(maxLength: 64, nullable: false),
                    Apellido = table.Column<string>(maxLength: 64, nullable: false),
                    AnoInicio = table.Column<uint>(nullable: false),
                    PensumCodigoCarrera = table.Column<uint>(nullable: true),
                    PensumAnoPensum = table.Column<uint>(nullable: true),
                    UsuarioEstudianteId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.EstudianteId);
                    table.UniqueConstraint("AK_Estudiantes_Carne", x => x.Carne);
                    table.ForeignKey(
                        name: "FK_Estudiantes_AspNetUsers_UsuarioEstudianteId",
                        column: x => x.UsuarioEstudianteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Estudiantes_Pensums_PensumCodigoCarrera_PensumAnoPensum",
                        columns: x => new { x.PensumCodigoCarrera, x.PensumAnoPensum },
                        principalTable: "Pensums",
                        principalColumns: new[] { "CodigoCarrera", "AnoPensum" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PensumCursos",
                columns: table => new
                {
                    CodigoCarrera = table.Column<uint>(nullable: false),
                    AnoPensum = table.Column<uint>(nullable: false),
                    CodigoCurso = table.Column<uint>(nullable: false),
                    Ciclo = table.Column<byte>(nullable: false),
                    Creditos = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PensumCursos", x => new { x.CodigoCarrera, x.AnoPensum, x.CodigoCurso });
                    table.ForeignKey(
                        name: "FK_PensumCursos_Pensums_CodigoCarrera_AnoPensum",
                        columns: x => new { x.CodigoCarrera, x.AnoPensum },
                        principalTable: "Pensums",
                        principalColumns: new[] { "CodigoCarrera", "AnoPensum" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PensumCursos_Cursos_CodigoCarrera_CodigoCurso",
                        columns: x => new { x.CodigoCarrera, x.CodigoCurso },
                        principalTable: "Cursos",
                        principalColumns: new[] { "CodigoCarrera", "CodigoCurso" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notas",
                columns: table => new
                {
                    Ano = table.Column<uint>(nullable: false),
                    EstudianteId = table.Column<uint>(nullable: false),
                    CodigoCarrera = table.Column<uint>(nullable: false),
                    AnoPensum = table.Column<uint>(nullable: false),
                    CodigoCurso = table.Column<uint>(nullable: false),
                    PrimerParcial = table.Column<byte>(nullable: false),
                    SegundoParcial = table.Column<byte>(nullable: false),
                    Actividades = table.Column<byte>(nullable: false),
                    Zona = table.Column<byte>(nullable: false, computedColumnSql: "PrimerParcial + SegundoParcial + Actividades"),
                    ExamenFinal = table.Column<byte>(nullable: false),
                    NotaFinal = table.Column<byte>(nullable: false, computedColumnSql: "Zona + ExamenFinal"),
                    Aprobado = table.Column<bool>(nullable: false, computedColumnSql: "NotaFinal >= 61")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notas", x => new { x.EstudianteId, x.CodigoCarrera, x.AnoPensum, x.CodigoCurso, x.Ano });
                    table.ForeignKey(
                        name: "FK_Notas_Estudiantes_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "Estudiantes",
                        principalColumn: "EstudianteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notas_PensumCursos_CodigoCarrera_AnoPensum_CodigoCurso",
                        columns: x => new { x.CodigoCarrera, x.AnoPensum, x.CodigoCurso },
                        principalTable: "PensumCursos",
                        principalColumns: new[] { "CodigoCarrera", "AnoPensum", "CodigoCurso" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_DeviceCode",
                table: "DeviceCodes",
                column: "DeviceCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_Expiration",
                table: "DeviceCodes",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_UsuarioEstudianteId",
                table: "Estudiantes",
                column: "UsuarioEstudianteId");

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_PensumCodigoCarrera_PensumAnoPensum",
                table: "Estudiantes",
                columns: new[] { "PensumCodigoCarrera", "PensumAnoPensum" });

            migrationBuilder.CreateIndex(
                name: "IX_Notas_CodigoCarrera_AnoPensum_CodigoCurso",
                table: "Notas",
                columns: new[] { "CodigoCarrera", "AnoPensum", "CodigoCurso" });

            migrationBuilder.CreateIndex(
                name: "IX_PensumCursos_CodigoCarrera_CodigoCurso",
                table: "PensumCursos",
                columns: new[] { "CodigoCarrera", "CodigoCurso" });

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_Expiration",
                table: "PersistedGrants",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_SubjectId_ClientId_Type",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DeviceCodes");

            migrationBuilder.DropTable(
                name: "Notas");

            migrationBuilder.DropTable(
                name: "PersistedGrants");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Estudiantes");

            migrationBuilder.DropTable(
                name: "PensumCursos");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Pensums");

            migrationBuilder.DropTable(
                name: "Cursos");

            migrationBuilder.DropTable(
                name: "Carreras");
        }
    }
}
