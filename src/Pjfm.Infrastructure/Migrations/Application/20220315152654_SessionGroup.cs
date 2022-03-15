using Microsoft.EntityFrameworkCore.Migrations;

namespace Pjfm.Infrastructure.Migrations.Application
{
    public partial class SessionGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionGroup",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserSessionGroup",
                columns: table => new
                {
                    FillerQueueParticipantGroupsId = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    FillerQueueParticipantsId = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserSessionGroup", x => new { x.FillerQueueParticipantGroupsId, x.FillerQueueParticipantsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserSessionGroup_AspNetUsers_FillerQueueParticipantsId",
                        column: x => x.FillerQueueParticipantsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserSessionGroup_SessionGroup_FillerQueueParticipantGroupsId",
                        column: x => x.FillerQueueParticipantGroupsId,
                        principalTable: "SessionGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserSessionGroup_FillerQueueParticipantsId",
                table: "ApplicationUserSessionGroup",
                column: "FillerQueueParticipantsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserSessionGroup");

            migrationBuilder.DropTable(
                name: "SessionGroup");
        }
    }
}
