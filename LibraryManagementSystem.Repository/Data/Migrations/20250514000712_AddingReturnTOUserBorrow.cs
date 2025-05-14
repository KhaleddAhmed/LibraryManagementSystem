using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddingReturnTOUserBorrow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReturnConfirmed",
                table: "UserBorrowings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UserWantsToReturn",
                table: "UserBorrowings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReturnConfirmed",
                table: "UserBorrowings");

            migrationBuilder.DropColumn(
                name: "UserWantsToReturn",
                table: "UserBorrowings");
        }
    }
}
