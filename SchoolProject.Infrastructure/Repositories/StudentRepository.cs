using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Helpers;
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
    public class StudentRepository : GenericRepositoryAsync<Student>,IStudentRepository
    {
        #region Fields
        //private readonly AppDbContext _context;
        private readonly DbSet<Student> _students;
        #endregion

        #region Constructor
        public StudentRepository(AppDbContext context) : base(context)
        {
            //_context = context;
            _students = context.Set<Student>();
        }


        #endregion

        #region Methods
        public async Task<List<Student>> GetStudentsListAsync()
        {
            // Add Include to load related Department data.
            return await _students.Include(x => x.Department).ToListAsync();
        }
        public IQueryable<Student> GetStudentsQueryable()
        {
            return _students.AsNoTracking().Include(x => x.Department).AsQueryable();
        }
        public IQueryable<Student> FilterStudentsPaginatedQueryable(string search, StudentOrderingEnum orderBy = StudentOrderingEnum.StudentId)
        {
            var queryable = _students.AsNoTracking().Include(x => x.Department).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                queryable = queryable.Where(x => x.Name.Contains(search) || x.Address.Contains(search));
            }

            switch (orderBy)
            {
                case StudentOrderingEnum.None:
                    queryable = queryable.OrderBy(x => x.StudId);
                    break;
                case StudentOrderingEnum.StudentId:
                    queryable = queryable.OrderBy(x => x.StudId);
                    break;
                case StudentOrderingEnum.StudentName:
                    queryable = queryable.OrderBy(x => x.Name);
                    break;
                case StudentOrderingEnum.StudentAddress:
                    queryable = queryable.OrderBy(x => x.Address);
                    break;
                case StudentOrderingEnum.DepartmentName:
                    queryable = queryable.OrderBy(x => x.Department.DeptName);
                    break;
                default:
                    queryable = queryable.OrderBy(x => x.StudId);
                    break;
            }

            return queryable;
        }
        public async Task<Student> GetStudentByIdWithIncludeAsync(int studentId)
        {
            var student = await _students.Include(x => x.Department)
                                       .FirstOrDefaultAsync(x => x.StudId == studentId);

            return student;
        }
        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _students.FirstOrDefaultAsync(x => x.StudId == studentId);
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, int id = 0)
        {
            var query = _students.AsNoTracking().AsQueryable();

            if (id != 0)
            {
                // UPDATE CASE: Check if name exists, BUT ignore the record with this specific ID
                query = query.Where(x => x.Name == name && x.StudId != id);
            }
            else
            {
                // ADD CASE: Just check if name exists
                query = query.Where(x => x.Name == name);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> IsStudentIdExist(int studentId)
        {
            return await _students.AnyAsync(x => x.StudId == studentId);
        }

        public IQueryable<Student> FilterStudentsByDepartmentIdPaginatedQueryable(int departmentId, string search = null, StudentOrderingEnum orderBy = StudentOrderingEnum.StudentId)
        {
            var queryable = _students.AsNoTracking().Where(s => s.DeptId == departmentId).AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                queryable = queryable.Where(x => x.Name.Contains(search) || x.Address.Contains(search));
            }

            switch (orderBy)
            {
                case StudentOrderingEnum.None:
                    queryable = queryable.OrderBy(x => x.StudId);
                    break;
                case StudentOrderingEnum.StudentId:
                    queryable = queryable.OrderBy(x => x.StudId);
                    break;
                case StudentOrderingEnum.StudentName:
                    queryable = queryable.OrderBy(x => x.Name);
                    break;
                case StudentOrderingEnum.StudentAddress:
                    queryable = queryable.OrderBy(x => x.Address);
                    break;
                case StudentOrderingEnum.DepartmentName:
                    queryable = queryable.OrderBy(x => x.Department.DeptName);
                    break;
                default:
                    queryable = queryable.OrderBy(x => x.StudId);
                    break;
            }

            return queryable;
        }




        #endregion
    }
}
