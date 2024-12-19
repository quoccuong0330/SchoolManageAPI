using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManageAPI.Migrations
{
    /// <inheritdoc />
    public partial class init_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "da0f0616-b052-4963-ad0a-140260caf394");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessToken", "Address", "ClassId", "Email", "ExpiresIn", "Name", "Password", "Phone", "RefreshToken", "RefreshTokenExpiry", "Role", "TableId", "YearOfBirth" },
                values: new object[] { "da0f0616-b052-4963-ad0a-140260caf394", null, "", null, "admin@example.com", null, "Admin", "$2a$11$ZT2z9/WkOnbfNl0FCPxzLOUUQ86ZxjbAAjenRziCg83x8PzUj7Za.", "", null, null, "admin", null, 0 });
        }
    }
}
