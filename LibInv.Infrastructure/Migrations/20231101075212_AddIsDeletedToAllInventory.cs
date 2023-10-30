using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibInventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToAllInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Inventory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Inventory");
        }
    }
}
