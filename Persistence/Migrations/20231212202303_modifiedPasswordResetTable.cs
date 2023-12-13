using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class modifiedPasswordResetTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordResetRequest_AppUser_AppUserId",
                table: "PasswordResetRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PasswordResetRequest",
                table: "PasswordResetRequest");

            migrationBuilder.RenameTable(
                name: "PasswordResetRequest",
                newName: "PasswordResetRequests");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordResetRequest_AppUserId",
                table: "PasswordResetRequests",
                newName: "IX_PasswordResetRequests_AppUserId");

            migrationBuilder.AddColumn<int>(
                name: "CodeEntryCount",
                table: "PasswordResetRequests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsExpiredOrFailed",
                table: "PasswordResetRequests",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PasswordResetRequests",
                table: "PasswordResetRequests",
                column: "PasswordRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResetRequests_AppUser_AppUserId",
                table: "PasswordResetRequests",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PasswordResetRequests_AppUser_AppUserId",
                table: "PasswordResetRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PasswordResetRequests",
                table: "PasswordResetRequests");

            migrationBuilder.DropColumn(
                name: "CodeEntryCount",
                table: "PasswordResetRequests");

            migrationBuilder.DropColumn(
                name: "IsExpiredOrFailed",
                table: "PasswordResetRequests");

            migrationBuilder.RenameTable(
                name: "PasswordResetRequests",
                newName: "PasswordResetRequest");

            migrationBuilder.RenameIndex(
                name: "IX_PasswordResetRequests_AppUserId",
                table: "PasswordResetRequest",
                newName: "IX_PasswordResetRequest_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PasswordResetRequest",
                table: "PasswordResetRequest",
                column: "PasswordRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_PasswordResetRequest_AppUser_AppUserId",
                table: "PasswordResetRequest",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
