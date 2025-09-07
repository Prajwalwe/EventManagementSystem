using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karya2.Migrations
{
    /// <inheritdoc />
    public partial class StudentAndCollegeLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollegeLogin",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    CollegeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollegeLogin", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "StudentLogins",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentLogins", x => x.UserName);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollegeLogin");

            migrationBuilder.DropTable(
                name: "StudentLogins");
        }
    }
}
