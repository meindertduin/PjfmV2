using Microsoft.EntityFrameworkCore.Migrations;

namespace Pjfm.Infrastructure.Migrations.Application
{
    public partial class AddSpotifyGebruikersData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SpotifyGebruikersData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GebruikerId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpotifyGebruikersData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpotifyNummer_GebruikerId",
                table: "SpotifyNummer",
                column: "GebruikerId");

            migrationBuilder.CreateIndex(
                name: "IX_SpotifyGebruikersData_GebruikerId",
                table: "SpotifyGebruikersData",
                column: "GebruikerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpotifyGebruikersData");

            migrationBuilder.DropIndex(
                name: "IX_SpotifyNummer_GebruikerId",
                table: "SpotifyNummer");
        }
    }
}
