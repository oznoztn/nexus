using Microsoft.EntityFrameworkCore.Migrations;

namespace Nexus.Data.Migrations
{
    public partial class BookCourseUrlProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Course",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Book",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Book");
        }
    }
}
