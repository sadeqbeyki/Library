using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addIsApporvedLoans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Borrows",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Borrows");
        }
    }
}
