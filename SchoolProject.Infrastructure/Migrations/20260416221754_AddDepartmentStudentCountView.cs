using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentStudentCountView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE VIEW vw_DepartmentStudentCount AS
        Select 
            Departments.DeptId,
            Departments.DeptName,
            (Select COUNT(*) from Students Where Students.DeptId = Departments.DeptId) As StudentsInDepartment
        from Departments
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // PASTE THIS HERE:
            migrationBuilder.Sql("DROP VIEW vw_DepartmentStudentCount");
        }
    }
}
