using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateInstructorSalaryProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE PROCEDURE sp_UpdateInstructorSalary
            @InstructorId INT,
            @NewSalary DECIMAL(18,2)
        AS
        BEGIN
            UPDATE Instructors
            SET Salary = @NewSalary
            WHERE Id = @InstructorId
        END
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE sp_UpdateInstructorSalary");
        }
    }
}
