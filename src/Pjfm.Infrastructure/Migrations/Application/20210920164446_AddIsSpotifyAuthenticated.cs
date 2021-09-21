using Microsoft.EntityFrameworkCore.Migrations;

namespace Pjfm.Infrastructure.Migrations.Application
{
    public partial class AddIsSpotifyAuthenticated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SpotifyAuthenticated",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpotifyAuthenticated",
                table: "AspNetUsers");
        }
    }
}
