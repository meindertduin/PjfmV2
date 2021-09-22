using Microsoft.EntityFrameworkCore.Migrations;

namespace Pjfm.Infrastructure.Migrations.Application
{
    public partial class CascadeAlbumsOnDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpotifyTrack_SpotifyAlbum_SpotifyAlbumId",
                table: "SpotifyTrack");

            migrationBuilder.AddForeignKey(
                name: "FK_SpotifyTrack_SpotifyAlbum_SpotifyAlbumId",
                table: "SpotifyTrack",
                column: "SpotifyAlbumId",
                principalTable: "SpotifyAlbum",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpotifyTrack_SpotifyAlbum_SpotifyAlbumId",
                table: "SpotifyTrack");

            migrationBuilder.AddForeignKey(
                name: "FK_SpotifyTrack_SpotifyAlbum_SpotifyAlbumId",
                table: "SpotifyTrack",
                column: "SpotifyAlbumId",
                principalTable: "SpotifyAlbum",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
