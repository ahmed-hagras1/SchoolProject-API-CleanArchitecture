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
        #endregion
    }
}
