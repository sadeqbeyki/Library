using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initCategoryLimitation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BookCategories",
                type: "nvarchar(780)",
                maxLength: 780,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "BookCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(780)",
                oldMaxLength: 780,
                oldNullable: true);
        }
    }
}
