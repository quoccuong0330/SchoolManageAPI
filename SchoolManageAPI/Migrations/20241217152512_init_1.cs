using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class init_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tables_TableId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TableId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "TableId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TableId",
                table: "Users",
                column: "TableId",
                unique: true,
                filter: "[TableId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tables_TableId",
                table: "Users",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tables_TableId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TableId",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "TableId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TableId",
                table: "Users",
                column: "TableId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tables_TableId",
                table: "Users",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
