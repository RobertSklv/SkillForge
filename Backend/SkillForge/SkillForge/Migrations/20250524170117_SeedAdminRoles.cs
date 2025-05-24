using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SkillForge.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AdminRoles",
                columns: new[] { "Id", "Code", "CreatedAt", "DisplayedName", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "admin", null, "Administrator", null },
                    { 2, "mod", null, "Moderator", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdminRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AdminRoles",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
