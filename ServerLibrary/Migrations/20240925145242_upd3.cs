using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Migrations
{
    /// <inheritdoc />
    public partial class upd3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeneralDepartmentId",
                table: "GeneralDepartments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralDepartments_GeneralDepartmentId",
                table: "GeneralDepartments",
                column: "GeneralDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralDepartments_GeneralDepartments_GeneralDepartmentId",
                table: "GeneralDepartments",
                column: "GeneralDepartmentId",
                principalTable: "GeneralDepartments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralDepartments_GeneralDepartments_GeneralDepartmentId",
                table: "GeneralDepartments");

            migrationBuilder.DropIndex(
                name: "IX_GeneralDepartments_GeneralDepartmentId",
                table: "GeneralDepartments");

            migrationBuilder.DropColumn(
                name: "GeneralDepartmentId",
                table: "GeneralDepartments");
        }
    }
}
