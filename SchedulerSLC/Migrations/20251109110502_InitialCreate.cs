using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchedulerSLC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    place = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "Events_KeyHolders",
                columns: table => new
                {
                    EventsAsKeyHolderName = table.Column<string>(type: "text", nullable: false),
                    KeyHoldersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events_KeyHolders", x => new { x.EventsAsKeyHolderName, x.KeyHoldersId });
                    table.ForeignKey(
                        name: "FK_Events_KeyHolders_Events_EventsAsKeyHolderName",
                        column: x => x.EventsAsKeyHolderName,
                        principalTable: "Events",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_KeyHolders_Participants_KeyHoldersId",
                        column: x => x.KeyHoldersId,
                        principalTable: "Participants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events_Participants",
                columns: table => new
                {
                    EventsAsParticipantName = table.Column<string>(type: "text", nullable: false),
                    ParticipantsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events_Participants", x => new { x.EventsAsParticipantName, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_Events_Participants_Events_EventsAsParticipantName",
                        column: x => x.EventsAsParticipantName,
                        principalTable: "Events",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_Participants_Participants_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Participants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.id);
                    table.ForeignKey(
                        name: "FK_Groups_Participants_id",
                        column: x => x.id,
                        principalTable: "Participants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    patronymic = table.Column<string>(type: "text", nullable: true),
                    role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Participants_id",
                        column: x => x.id,
                        principalTable: "Participants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users_Groups",
                columns: table => new
                {
                    GroupsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_Groups", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_Users_Groups_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Groups_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users_Roles",
                columns: table => new
                {
                    RolesName = table.Column<string>(type: "text", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_Roles", x => new { x.RolesName, x.UsersId });
                    table.ForeignKey(
                        name: "FK_Users_Roles_Roles_RolesName",
                        column: x => x.RolesName,
                        principalTable: "Roles",
                        principalColumn: "name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_KeyHolders_KeyHoldersId",
                table: "Events_KeyHolders",
                column: "KeyHoldersId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_Participants_ParticipantsId",
                table: "Events_Participants",
                column: "ParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Groups_UsersId",
                table: "Users_Groups",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Roles_UsersId",
                table: "Users_Roles",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events_KeyHolders");

            migrationBuilder.DropTable(
                name: "Events_Participants");

            migrationBuilder.DropTable(
                name: "Users_Groups");

            migrationBuilder.DropTable(
                name: "Users_Roles");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Participants");
        }
    }
}
