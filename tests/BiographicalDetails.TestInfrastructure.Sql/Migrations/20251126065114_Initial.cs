using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BiographicalDetails.TestInfrastructure.Sql.Migrations
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BiographicalDatas");
        }
    }
}
