using Microsoft.EntityFrameworkCore.Migrations;

namespace Lingo.Migrations
{
    public partial class AddPassedToReviewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Passed",
                table: "Reviews",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Passed",
                table: "Reviews");
        }
    }
}
