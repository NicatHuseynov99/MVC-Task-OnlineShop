using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvsTaskOnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProductTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ColorProduct_Colors_ColorId",
                table: "ColorProduct");

            migrationBuilder.RenameColumn(
                name: "ColorId",
                table: "ColorProduct",
                newName: "ColorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ColorProduct_Colors_ColorsId",
                table: "ColorProduct",
                column: "ColorsId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ColorProduct_Colors_ColorsId",
                table: "ColorProduct");

            migrationBuilder.RenameColumn(
                name: "ColorsId",
                table: "ColorProduct",
                newName: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ColorProduct_Colors_ColorId",
                table: "ColorProduct",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
