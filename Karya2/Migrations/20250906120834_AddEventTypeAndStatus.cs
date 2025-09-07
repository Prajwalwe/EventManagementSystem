using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karya2.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTypeAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventStatus",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventTypeId",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    EventTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.EventTypeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropColumn(
                name: "EventStatus",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventTypeId",
                table: "Events");
        }
    }
}
