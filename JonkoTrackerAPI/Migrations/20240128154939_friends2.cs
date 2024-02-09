using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JonkoTrackerAPI.Migrations
{
    /// <inheritdoc />
    public partial class friends2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_Users_FriendsId",
                table: "UserUser");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_Users_UserId",
                table: "UserUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserUser",
                table: "UserUser");

            migrationBuilder.RenameTable(
                name: "UserUser",
                newName: "Friends");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Friends",
                newName: "FriendsOfId");

            migrationBuilder.RenameIndex(
                name: "IX_UserUser_UserId",
                table: "Friends",
                newName: "IX_Friends_FriendsOfId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friends",
                table: "Friends",
                columns: new[] { "FriendsId", "FriendsOfId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_FriendsId",
                table: "Friends",
                column: "FriendsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_Users_FriendsOfId",
                table: "Friends",
                column: "FriendsOfId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_FriendsId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_Users_FriendsOfId",
                table: "Friends");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friends",
                table: "Friends");

            migrationBuilder.RenameTable(
                name: "Friends",
                newName: "UserUser");

            migrationBuilder.RenameColumn(
                name: "FriendsOfId",
                table: "UserUser",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_FriendsOfId",
                table: "UserUser",
                newName: "IX_UserUser_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserUser",
                table: "UserUser",
                columns: new[] { "FriendsId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_Users_FriendsId",
                table: "UserUser",
                column: "FriendsId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_Users_UserId",
                table: "UserUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
