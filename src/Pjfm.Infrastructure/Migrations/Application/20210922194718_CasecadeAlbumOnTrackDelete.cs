using Microsoft.EntityFrameworkCore.Migrations;

namespace Pjfm.Infrastructure.Migrations.Application
{
    public partial class CasecadeAlbumOnTrackDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpotifyTrack_SpotifyAlbum_SpotifyAlbumId",
                table: "SpotifyTrack");

            migrationBuilder.DropIndex(
                name: "IX_SpotifyTrack_SpotifyAlbumId",
                table: "SpotifyTrack");

            migrationBuilder.DropColumn(
                name: "SpotifyAlbumId",
                table: "SpotifyTrack");

            migrationBuilder.AddColumn<int>(
                name: "SpotifyTrackId",
                table: "SpotifyAlbum",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SpotifyAlbum_SpotifyTrackId",
                table: "SpotifyAlbum",
                column: "SpotifyTrackId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SpotifyAlbum_SpotifyTrack_SpotifyTrackId",
                table: "SpotifyAlbum",
                column: "SpotifyTrackId",
                principalTable: "SpotifyTrack",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpotifyAlbum_SpotifyTrack_SpotifyTrackId",
                table: "SpotifyAlbum");

            migrationBuilder.DropIndex(
                name: "IX_SpotifyAlbum_SpotifyTrackId",
                table: "SpotifyAlbum");

            migrationBuilder.DropColumn(
                name: "SpotifyTrackId",
                table: "SpotifyAlbum");

            migrationBuilder.AddColumn<int>(
                name: "SpotifyAlbumId",
                table: "SpotifyTrack",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SpotifyTrack_SpotifyAlbumId",
                table: "SpotifyTrack",
                column: "SpotifyAlbumId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SpotifyTrack_SpotifyAlbum_SpotifyAlbumId",
                table: "SpotifyTrack",
                column: "SpotifyAlbumId",
                principalTable: "SpotifyAlbum",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
