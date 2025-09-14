using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updateGradeuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_AspNetUsers_GradedBy",
                table: "Grades");

            migrationBuilder.DropIndex(
                name: "IX_Grades_GradedBy",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "GradedBy",
                table: "Grades");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradedBy",
                table: "Grades",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Grades_GradedBy",
                table: "Grades",
                column: "GradedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Grades_AspNetUsers_GradedBy",
                table: "Grades",
                column: "GradedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
