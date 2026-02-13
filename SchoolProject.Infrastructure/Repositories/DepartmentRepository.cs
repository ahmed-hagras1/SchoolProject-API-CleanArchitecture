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
    public class DepartmentRepository : GenericRepositoryAsync<Department>, IDepartmentRepository
    {
        #region Fields / Properties
        private readonly DbSet<Department> _departments;
        #endregion

        #region Constructor(s)
        public DepartmentRepository(AppDbContext context) : base(context)
        {
            _departments = context.Set<Department>();
        }


        #endregion

        #region Methods
        // Added AsNoTracking() for  performance
        // Used expression body (=>) for cleaner syntax
        public async Task<List<Department>> GetDepartmentsListAsync() =>
            await _departments
                .Include(x => x.InstructorManager)
                .AsNoTracking()
                .ToListAsync();

        // Added AsSplitQuery() to prevent "Cartesian Explosion" with multiple collections
        // Added AsNoTracking() to improve performance since we are only reading data
        public async Task<Department> GetDepartmentsByIdWithIncludeAsync(int departmentId) =>
            await _departments
                .Include(x => x.InstructorManager)
                .Include(x => x.DepartmentSubjects).ThenInclude(x => x.Subject)
                .Include(x => x.Instructors)
                .AsSplitQuery()  // <--- Very Important for multiple Includes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.DeptId == departmentId);

        public async Task<bool> IsDepartmentExist(int? departmentId) => await _departments.AnyAsync(x => x.DeptId == departmentId);
        #endregion
    }
}
