using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BorrowStatus",
                table: "UserBorrowings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "UserBorrowings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvaliable",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowStatus",
                table: "UserBorrowings");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "UserBorrowings");

            migrationBuilder.DropColumn(
                name: "IsAvaliable",
                table: "Books");
        }
    }
}
