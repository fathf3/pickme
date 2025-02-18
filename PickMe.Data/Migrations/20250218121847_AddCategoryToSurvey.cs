using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PickMe.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToSurvey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Surveys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Surveys_CategoryId",
                table: "Surveys",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Surveys_Category_CategoryId",
                table: "Surveys",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surveys_Category_CategoryId",
                table: "Surveys");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Surveys_CategoryId",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Surveys");
        }
    }
}
