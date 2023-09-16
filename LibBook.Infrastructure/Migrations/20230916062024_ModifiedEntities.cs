using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibBook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorBooks_Authors_AuthorId",
                table: "AuthorBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorBooks_Books_AuthorBookId",
                table: "AuthorBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_PublisherBooks_Books_PublisherBookId",
                table: "PublisherBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_PublisherBooks_Publishers_PublisherId",
                table: "PublisherBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_TranslatorBooks_Books_TranslatorBookId",
                table: "TranslatorBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_TranslatorBooks_Translators_TranslatorId",
                table: "TranslatorBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TranslatorBooks",
                table: "TranslatorBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PublisherBooks",
                table: "PublisherBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorBooks",
                table: "AuthorBooks");

            migrationBuilder.RenameTable(
                name: "TranslatorBooks",
                newName: "BookTranslators");

            migrationBuilder.RenameTable(
                name: "PublisherBooks",
                newName: "BookPublishers");

            migrationBuilder.RenameTable(
                name: "AuthorBooks",
                newName: "BookAuthors");

            migrationBuilder.RenameIndex(
                name: "IX_TranslatorBooks_TranslatorBookId",
                table: "BookTranslators",
                newName: "IX_BookTranslators_TranslatorBookId");

            migrationBuilder.RenameIndex(
                name: "IX_PublisherBooks_PublisherBookId",
                table: "BookPublishers",
                newName: "IX_BookPublishers_PublisherBookId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorBooks_AuthorBookId",
                table: "BookAuthors",
                newName: "IX_BookAuthors_AuthorBookId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Translators",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Translators",
                type: "nvarchar(750)",
                maxLength: 750,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Publishers",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Publishers",
                type: "nvarchar(750)",
                maxLength: 750,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Books",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Books",
                type: "nvarchar(750)",
                maxLength: 750,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Authors",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Authors",
                type: "nvarchar(750)",
                maxLength: 750,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookTranslators",
                table: "BookTranslators",
                columns: new[] { "TranslatorId", "TranslatorBookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookPublishers",
                table: "BookPublishers",
                columns: new[] { "PublisherId", "PublisherBookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookAuthors",
                table: "BookAuthors",
                columns: new[] { "AuthorId", "AuthorBookId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_Authors_AuthorId",
                table: "BookAuthors",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_Books_AuthorBookId",
                table: "BookAuthors",
                column: "AuthorBookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookPublishers_Books_PublisherBookId",
                table: "BookPublishers",
                column: "PublisherBookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookPublishers_Publishers_PublisherId",
                table: "BookPublishers",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookTranslators_Books_TranslatorBookId",
                table: "BookTranslators",
                column: "TranslatorBookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookTranslators_Translators_TranslatorId",
                table: "BookTranslators",
                column: "TranslatorId",
                principalTable: "Translators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_Authors_AuthorId",
                table: "BookAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_Books_AuthorBookId",
                table: "BookAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookPublishers_Books_PublisherBookId",
                table: "BookPublishers");

            migrationBuilder.DropForeignKey(
                name: "FK_BookPublishers_Publishers_PublisherId",
                table: "BookPublishers");

            migrationBuilder.DropForeignKey(
                name: "FK_BookTranslators_Books_TranslatorBookId",
                table: "BookTranslators");

            migrationBuilder.DropForeignKey(
                name: "FK_BookTranslators_Translators_TranslatorId",
                table: "BookTranslators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookTranslators",
                table: "BookTranslators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookPublishers",
                table: "BookPublishers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookAuthors",
                table: "BookAuthors");

            migrationBuilder.RenameTable(
                name: "BookTranslators",
                newName: "TranslatorBooks");

            migrationBuilder.RenameTable(
                name: "BookPublishers",
                newName: "PublisherBooks");

            migrationBuilder.RenameTable(
                name: "BookAuthors",
                newName: "AuthorBooks");

            migrationBuilder.RenameIndex(
                name: "IX_BookTranslators_TranslatorBookId",
                table: "TranslatorBooks",
                newName: "IX_TranslatorBooks_TranslatorBookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookPublishers_PublisherBookId",
                table: "PublisherBooks",
                newName: "IX_PublisherBooks_PublisherBookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookAuthors_AuthorBookId",
                table: "AuthorBooks",
                newName: "IX_AuthorBooks_AuthorBookId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Translators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Translators",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(750)",
                oldMaxLength: 750);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Publishers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(750)",
                oldMaxLength: 750);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Books",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Books",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(750)",
                oldMaxLength: 750);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(750)",
                oldMaxLength: 750);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TranslatorBooks",
                table: "TranslatorBooks",
                columns: new[] { "TranslatorId", "TranslatorBookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublisherBooks",
                table: "PublisherBooks",
                columns: new[] { "PublisherId", "PublisherBookId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorBooks",
                table: "AuthorBooks",
                columns: new[] { "AuthorId", "AuthorBookId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorBooks_Authors_AuthorId",
                table: "AuthorBooks",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorBooks_Books_AuthorBookId",
                table: "AuthorBooks",
                column: "AuthorBookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublisherBooks_Books_PublisherBookId",
                table: "PublisherBooks",
                column: "PublisherBookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublisherBooks_Publishers_PublisherId",
                table: "PublisherBooks",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TranslatorBooks_Books_TranslatorBookId",
                table: "TranslatorBooks",
                column: "TranslatorBookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TranslatorBooks_Translators_TranslatorId",
                table: "TranslatorBooks",
                column: "TranslatorId",
                principalTable: "Translators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
