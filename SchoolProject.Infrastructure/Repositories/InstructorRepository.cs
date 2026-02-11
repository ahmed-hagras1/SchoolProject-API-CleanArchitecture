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
        #endregion
    }
}
