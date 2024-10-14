using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kantines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stad = table.Column<int>(type: "int", nullable: false),
                    locatie = table.Column<int>(type: "int", nullable: false),
                    warm = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kantines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Producten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    alcohol = table.Column<bool>(type: "bit", nullable: false),
                    foto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    studentnummer = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    geboortedatum = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medewerkers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    persooneelsNr = table.Column<int>(type: "int", nullable: false),
                    kantineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medewerkers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medewerkers_Kantines_kantineId",
                        column: x => x.kantineId,
                        principalTable: "Kantines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pakketten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    stad = table.Column<int>(type: "int", nullable: false),
                    kantineId = table.Column<int>(type: "int", nullable: false),
                    pickup = table.Column<DateTime>(type: "datetime2", nullable: false),
                    pickUpMax = table.Column<DateTime>(type: "datetime2", nullable: false),
                    volwassen = table.Column<bool>(type: "bit", nullable: false),
                    prijs = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    maaltijd = table.Column<int>(type: "int", nullable: false),
                    gereserveerdId = table.Column<int>(type: "int", nullable: true),
                    warm = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pakketten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pakketten_Kantines_kantineId",
                        column: x => x.kantineId,
                        principalTable: "Kantines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pakketten_Students_gereserveerdId",
                        column: x => x.gereserveerdId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PakketProduct",
                columns: table => new
                {
                    pakkettenId = table.Column<int>(type: "int", nullable: false),
                    productenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PakketProduct", x => new { x.pakkettenId, x.productenId });
                    table.ForeignKey(
                        name: "FK_PakketProduct_Pakketten_pakkettenId",
                        column: x => x.pakkettenId,
                        principalTable: "Pakketten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PakketProduct_Producten_productenId",
                        column: x => x.productenId,
                        principalTable: "Producten",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medewerkers_kantineId",
                table: "Medewerkers",
                column: "kantineId");

            migrationBuilder.CreateIndex(
                name: "IX_PakketProduct_productenId",
                table: "PakketProduct",
                column: "productenId");

            migrationBuilder.CreateIndex(
                name: "IX_Pakketten_gereserveerdId",
                table: "Pakketten",
                column: "gereserveerdId");

            migrationBuilder.CreateIndex(
                name: "IX_Pakketten_kantineId",
                table: "Pakketten",
                column: "kantineId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Medewerkers");

            migrationBuilder.DropTable(
                name: "PakketProduct");

            migrationBuilder.DropTable(
                name: "Pakketten");

            migrationBuilder.DropTable(
                name: "Producten");

            migrationBuilder.DropTable(
                name: "Kantines");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
