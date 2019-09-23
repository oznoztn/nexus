using Microsoft.EntityFrameworkCore.Migrations;

namespace Nexus.Data.Migrations
{
    public partial class TagSlugFieldAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Tag",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Tag");
        }
    }
}
