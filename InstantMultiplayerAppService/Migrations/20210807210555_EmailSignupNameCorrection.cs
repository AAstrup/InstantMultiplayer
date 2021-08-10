using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMultiplayerAppService.Migrations
{
    public partial class EmailSignupNameCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "EmailSignups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailSignups",
                table: "EmailSignups",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailSignups",
                table: "EmailSignups");

            migrationBuilder.RenameTable(
                name: "EmailSignups",
                newName: "Students");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "Id");
        }
    }
}
