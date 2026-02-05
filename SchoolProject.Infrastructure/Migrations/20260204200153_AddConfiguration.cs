using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Instructor_InstructorManagerId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_Departments_DeptId",
                table: "Instructor");

            migrationBuilder.DropIndex(
                name: "IX_Departments_InstructorManagerId",
                table: "Departments");

            migrationBuilder.AlterColumn<int>(
                name: "InstructorManagerId",
                table: "Departments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_InstructorManagerId",
                table: "Departments",
                column: "InstructorManagerId",
                unique: true,
                filter: "[InstructorManagerId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Instructor_InstructorManagerId",
                table: "Departments",
                column: "InstructorManagerId",
                principalTable: "Instructor",
                principalColumn: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_Departments_DeptId",
                table: "Instructor",
                column: "DeptId",
                principalTable: "Departments",
                principalColumn: "DeptId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Instructor_InstructorManagerId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructor_Departments_DeptId",
                table: "Instructor");

            migrationBuilder.DropIndex(
                name: "IX_Departments_InstructorManagerId",
                table: "Departments");

            migrationBuilder.AlterColumn<int>(
                name: "InstructorManagerId",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_InstructorManagerId",
                table: "Departments",
                column: "InstructorManagerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Instructor_InstructorManagerId",
                table: "Departments",
                column: "InstructorManagerId",
                principalTable: "Instructor",
                principalColumn: "InstructorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructor_Departments_DeptId",
                table: "Instructor",
                column: "DeptId",
                principalTable: "Departments",
                principalColumn: "DeptId");
        }
    }
}
