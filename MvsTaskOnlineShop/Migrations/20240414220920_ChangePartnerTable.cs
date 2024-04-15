using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvsTaskOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class ChangePartnerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Partners",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Partners");
        }
    }
}
