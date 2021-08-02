using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pjfm.Infrastructure.Migrations.Application
{
    public partial class AddSpotifyTrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gebruiker");

            migrationBuilder.CreateTable(
                name: "SpotifyNummer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    GebruikerId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SpotifyNummerId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Artists = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TrackTermijn = table.Column<int>(type: "int", nullable: false),
                    NummerDuurMs = table.Column<int>(type: "int", nullable: false),
                    AangemaaktOp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotifyNummer", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpotifyNummer");

            migrationBuilder.CreateTable(
                name: "Gebruiker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GebruikersNaam = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: false),
                    IdentityUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gebruiker", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gebruiker_IdentityUserId",
                table: "Gebruiker",
                column: "IdentityUserId",
                unique: true);
        }
    }
}
