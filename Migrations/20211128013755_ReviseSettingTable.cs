using Microsoft.EntityFrameworkCore.Migrations;

namespace Lingo.Migrations
{
    public partial class ReviseSettingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishValue",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MalayValue",
                table: "Settings");

            migrationBuilder.CreateTable(
                name: "ReviewSettings",
                columns: table => new
                {
                    ReviewSettingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Language = table.Column<string>(type: "TEXT", nullable: true),
                    PassedThreshold = table.Column<int>(type: "INTEGER", nullable: false),
                    VocabsPerDay = table.Column<int>(type: "INTEGER", nullable: false),
                    LastVocabId = table.Column<int>(type: "INTEGER", nullable: false),
                    VocabsCategory = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewSettings", x => x.ReviewSettingId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewSettings");

            migrationBuilder.AddColumn<string>(
                name: "EnglishValue",
                table: "Settings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MalayValue",
                table: "Settings",
                type: "TEXT",
                nullable: true);
        }
    }
}
