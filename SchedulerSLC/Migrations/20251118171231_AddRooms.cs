using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchedulerSLC.Migrations
{
    /// <inheritdoc />
    public partial class AddRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "place",
                table: "Events",
                newName: "room");

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_room",
                table: "Events",
                column: "room");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Rooms_room",
                table: "Events",
                column: "room",
                principalTable: "Rooms",
                principalColumn: "name",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Rooms_room",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Events_room",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "room",
                table: "Events",
                newName: "place");
        }
    }
}
