using Microsoft.EntityFrameworkCore.Migrations;

namespace Nexus.Data.Migrations
{
    public partial class Tag_IsHidden_ColumnAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Tag",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Tag");
        }
    }
}
