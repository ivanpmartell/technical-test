using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BiographicalDetails.Infrastructure.Sql.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BiographicalDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PreferredPronouns = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LevelOfStudy = table.Column<int>(type: "int", nullable: false),
                    ImmigrationStatus = table.Column<int>(type: "int", nullable: false),
                    SocialInsuranceNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UniqueClientIdentifier = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiographicalDatas", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BiographicalDatas",
                columns: new[] { "Id", "Email", "FirstName", "ImmigrationStatus", "LastName", "LevelOfStudy", "PreferredPronouns", "SocialInsuranceNumber", "UniqueClientIdentifier" },
                values: new object[,]
                {
                    { 1, "ivan@test.com", "Ivan", 6, "Perez", 0, "He/Him", null, "0000-0000" },
                    { 2, "juan@yay.com", "Juan", 0, "Perez", 1, "They/Them", "000-000-000", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BiographicalDatas");
        }
    }
}
