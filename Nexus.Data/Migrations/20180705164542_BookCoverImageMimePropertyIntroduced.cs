using Microsoft.EntityFrameworkCore.Migrations;

namespace Nexus.Data.Migrations
{
    public partial class BookCoverImageMimePropertyIntroduced : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImageMime",
                table: "Book",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageMime",
                table: "Book");
        }
    }
}
