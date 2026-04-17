using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.InfrastructureBases;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrastructure.Repositories
{
    public class InstructorRepository : GenericRepositoryAsync<Instructor>, IInstructorRepository
    {
        #region Fields / Properties
        private readonly DbSet<Instructor> _instructors;
        #endregion

        #region Constructor(s)
        public InstructorRepository(AppDbContext context) : base(context)
        {
            _instructors = context.Set<Instructor>();

        }


        #endregion

        #region Methods
        public async Task<int> UpdateInstructorSalaryAsync(int instructorId, decimal newSalary)
        {
            // ExecuteSqlInterpolatedAsync is used for INSERT, UPDATE, DELETE stored procedures
            // It returns the number of rows affected.
            var rowsAffected = await _dbContext.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC sp_UpdateInstructorSalary @InstructorId = {instructorId}, @NewSalary = {newSalary}"
            );

            return rowsAffected;
        }
        #endregion
    }
}
