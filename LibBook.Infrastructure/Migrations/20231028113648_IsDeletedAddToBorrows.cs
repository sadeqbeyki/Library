using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IsDeletedAddToBorrows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Borrows",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Borrows");
        }
    }
}
