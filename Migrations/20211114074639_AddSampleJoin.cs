using Microsoft.EntityFrameworkCore.Migrations;

namespace Lingo.Migrations
{
    public partial class AddSampleJoin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Essays_SampleId",
                table: "Essays",
                column: "SampleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Essays_Samples_SampleId",
                table: "Essays",
                column: "SampleId",
                principalTable: "Samples",
                principalColumn: "SampleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Essays_Samples_SampleId",
                table: "Essays");

            migrationBuilder.DropIndex(
                name: "IX_Essays_SampleId",
                table: "Essays");
        }
    }
}
