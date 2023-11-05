using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAPI.Infrastructure.Migrations
{
    public partial class CreateRoomParticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Room_RoomId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_UserId",
                table: "Message");

            migrationBuilder.DropTable(
                name: "RoomUser");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Message",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_UserId",
                table: "Message",
                newName: "IX_Message_SenderId");

            migrationBuilder.AddColumn<DateTime>(
                name: "SendTime",
                table: "Message",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participant_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Room_Name",
                table: "Room",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participant_RoomId",
                table: "Participant",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Participant_UserId_RoomId",
                table: "Participant",
                columns: new[] { "UserId", "RoomId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Participant_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "Participant",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Room_RoomId",
                table: "Message",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Participant_SenderId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Room_RoomId",
                table: "Message");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.DropIndex(
                name: "IX_Room_Name",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "SendTime",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Message",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                newName: "IX_Message_UserId");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    RefreshTokenExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshTokenIsRevoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomUser",
                columns: table => new
                {
                    ParticipantsId = table.Column<int>(type: "int", nullable: false),
                    UserRoomsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomUser", x => new { x.ParticipantsId, x.UserRoomsId });
                    table.ForeignKey(
                        name: "FK_RoomUser_Room_UserRoomsId",
                        column: x => x.UserRoomsId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomUser_User_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomUser_UserRoomsId",
                table: "RoomUser",
                column: "UserRoomsId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Name",
                table: "User",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Room_RoomId",
                table: "Message",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_UserId",
                table: "Message",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
