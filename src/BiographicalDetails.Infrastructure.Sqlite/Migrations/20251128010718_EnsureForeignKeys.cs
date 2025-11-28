using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiographicalDetails.Infrastructure.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class EnsureForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserUcis_UserId",
                table: "UserUcis",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSins_UserId",
                table: "UserSins",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPronouns_UserId",
                table: "UserPronouns",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPronouns_Users_UserId",
                table: "UserPronouns",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSins_Users_UserId",
                table: "UserSins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUcis_Users_UserId",
                table: "UserUcis",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPronouns_Users_UserId",
                table: "UserPronouns");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSins_Users_UserId",
                table: "UserSins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUcis_Users_UserId",
                table: "UserUcis");

            migrationBuilder.DropIndex(
                name: "IX_UserUcis_UserId",
                table: "UserUcis");

            migrationBuilder.DropIndex(
                name: "IX_UserSins_UserId",
                table: "UserSins");

            migrationBuilder.DropIndex(
                name: "IX_UserPronouns_UserId",
                table: "UserPronouns");
        }
    }
}
