using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class init_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Users_EditorId",
                table: "Tables");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Users_EditorId",
                table: "Tables",
                column: "EditorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_Users_EditorId",
                table: "Tables");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_Users_EditorId",
                table: "Tables",
                column: "EditorId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
