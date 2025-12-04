using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchedulerSLC.Migrations
{
    /// <inheritdoc />
    public partial class AddIdToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_KeyHolders_Events_EventsAsKeyHolderName",
                table: "Events_KeyHolders");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Participants_Events_EventsAsParticipantName",
                table: "Events_Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events_Participants",
                table: "Events_Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events_KeyHolders",
                table: "Events_KeyHolders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventsAsParticipantName",
                table: "Events_Participants");

            migrationBuilder.DropColumn(
                name: "EventsAsKeyHolderName",
                table: "Events_KeyHolders");

            migrationBuilder.AddColumn<Guid>(
                name: "EventsAsParticipantId",
                table: "Events_Participants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EventsAsKeyHolderId",
                table: "Events_KeyHolders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "Events",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events_Participants",
                table: "Events_Participants",
                columns: new[] { "EventsAsParticipantId", "ParticipantsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events_KeyHolders",
                table: "Events_KeyHolders",
                columns: new[] { "EventsAsKeyHolderId", "KeyHoldersId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_KeyHolders_Events_EventsAsKeyHolderId",
                table: "Events_KeyHolders",
                column: "EventsAsKeyHolderId",
                principalTable: "Events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Participants_Events_EventsAsParticipantId",
                table: "Events_Participants",
                column: "EventsAsParticipantId",
                principalTable: "Events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_KeyHolders_Events_EventsAsKeyHolderId",
                table: "Events_KeyHolders");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Participants_Events_EventsAsParticipantId",
                table: "Events_Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events_Participants",
                table: "Events_Participants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events_KeyHolders",
                table: "Events_KeyHolders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventsAsParticipantId",
                table: "Events_Participants");

            migrationBuilder.DropColumn(
                name: "EventsAsKeyHolderId",
                table: "Events_KeyHolders");

            migrationBuilder.DropColumn(
                name: "id",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "EventsAsParticipantName",
                table: "Events_Participants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventsAsKeyHolderName",
                table: "Events_KeyHolders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events_Participants",
                table: "Events_Participants",
                columns: new[] { "EventsAsParticipantName", "ParticipantsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events_KeyHolders",
                table: "Events_KeyHolders",
                columns: new[] { "EventsAsKeyHolderName", "KeyHoldersId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "name");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_KeyHolders_Events_EventsAsKeyHolderName",
                table: "Events_KeyHolders",
                column: "EventsAsKeyHolderName",
                principalTable: "Events",
                principalColumn: "name",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Participants_Events_EventsAsParticipantName",
                table: "Events_Participants",
                column: "EventsAsParticipantName",
                principalTable: "Events",
                principalColumn: "name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
