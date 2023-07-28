using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialInventoryB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descriotion",
                table: "InventoryOperations",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "InventoryOperations",
                newName: "Descriotion");
        }
    }
}
