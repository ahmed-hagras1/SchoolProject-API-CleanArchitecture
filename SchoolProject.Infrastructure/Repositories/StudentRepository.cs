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
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        public StudentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<List<Student>> GetStudentsListAsync()
        {
            // Add Include to load related Department data.
            return await _context.Students.Include(x => x.Department).ToListAsync();
        }
        #endregion
    }
}
