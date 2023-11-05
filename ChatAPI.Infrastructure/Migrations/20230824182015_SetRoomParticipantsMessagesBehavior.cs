using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatAPI.Infrastructure.Migrations
{
    public partial class SetRoomParticipantsMessagesBehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Room_RoomId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_Room_RoomId",
                table: "Participant");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Room_RoomId",
                table: "Message",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_Room_RoomId",
                table: "Participant",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Room_RoomId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Participant_Room_RoomId",
                table: "Participant");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Room_RoomId",
                table: "Message",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Participant_Room_RoomId",
                table: "Participant",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id");
        }
    }
}
