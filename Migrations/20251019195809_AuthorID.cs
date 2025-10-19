using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solomon_Lorena_Lab2.Migrations
{
    /// <inheritdoc />
    public partial class AuthorID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Author_AuthorID1",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_AuthorID1",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "AuthorID1",
                table: "Book");

            migrationBuilder.AlterColumn<int>(
                name: "AuthorID",
                table: "Book",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorID",
                table: "Book",
                column: "AuthorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Author_AuthorID",
                table: "Book",
                column: "AuthorID",
                principalTable: "Author",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Author_AuthorID",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_AuthorID",
                table: "Book");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorID",
                table: "Book",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorID1",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_AuthorID1",
                table: "Book",
                column: "AuthorID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Author_AuthorID1",
                table: "Book",
                column: "AuthorID1",
                principalTable: "Author",
                principalColumn: "ID");
        }
    }
}
