using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karya2.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Feedbacks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Feedbacks");
        }
    }
}
