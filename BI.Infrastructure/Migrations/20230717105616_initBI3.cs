using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initBI3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BorrowId",
                table: "InventoryOperations",
                newName: "LendId");

            migrationBuilder.AlterColumn<string>(
                name: "OperatorId",
                table: "InventoryOperations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LendId",
                table: "InventoryOperations",
                newName: "BorrowId");

            migrationBuilder.AlterColumn<long>(
                name: "OperatorId",
                table: "InventoryOperations",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
