using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lingo.Migrations
{
    public partial class AddSampleTableAndUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Vocabs",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SampleId",
                table: "Essays",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Essays",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Samples",
                columns: table => new
                {
                    SampleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    Language = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.SampleId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Samples");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Vocabs");

            migrationBuilder.DropColumn(
                name: "SampleId",
                table: "Essays");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Essays");
        }
    }
}
