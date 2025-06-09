using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillForge.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreAggregateColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tags_TagId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TagId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "ArticlesCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TagFollowingsCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ArticlesCount",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticlesCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TagFollowingsCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ArticlesCount",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TagId",
                table: "Users",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tags_TagId",
                table: "Users",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
