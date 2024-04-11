using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvsTaskOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class ProductTableChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaregoryId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaregoryId",
                table: "Products");
        }
    }
}
