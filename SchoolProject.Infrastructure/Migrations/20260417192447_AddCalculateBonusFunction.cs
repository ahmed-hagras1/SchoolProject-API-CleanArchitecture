using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolProject.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCalculateBonusFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE FUNCTION fn_CalculateBonus (@salary DECIMAL(18,2))
        RETURNS DECIMAL(18,2)
        AS
        BEGIN
            -- Just a simple example: 10% bonus
            RETURN @salary * 0.10
        END
    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION fn_CalculateBonus");
        }
    }
}
