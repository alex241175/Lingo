using Microsoft.EntityFrameworkCore.Migrations;

namespace Lingo.Migrations
{
    public partial class AddUserRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vocabs_Essays_EssayId",
                table: "Vocabs");

            migrationBuilder.AlterColumn<int>(
                name: "EssayId",
                table: "Vocabs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vocabs_Essays_EssayId",
                table: "Vocabs",
                column: "EssayId",
                principalTable: "Essays",
                principalColumn: "EssayId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vocabs_Essays_EssayId",
                table: "Vocabs");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "EssayId",
                table: "Vocabs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vocabs_Essays_EssayId",
                table: "Vocabs",
                column: "EssayId",
                principalTable: "Essays",
                principalColumn: "EssayId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
