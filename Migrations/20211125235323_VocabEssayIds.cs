using Microsoft.EntityFrameworkCore.Migrations;

namespace Lingo.Migrations
{
    public partial class VocabEssayIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EssayIds",
                table: "Vocabs",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EssayIds",
                table: "Vocabs");
        }
    }
}
