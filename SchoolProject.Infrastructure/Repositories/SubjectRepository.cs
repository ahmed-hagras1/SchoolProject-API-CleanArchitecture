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
    public class SubjectRepository : GenericRepositoryAsync<Subject>, ISubjectRepository
    {
        #region Fields / Properties
        private readonly DbSet<Subject> _subjects;
        #endregion

        #region Constructor(s)
        public SubjectRepository(AppDbContext context) : base(context)
        {
            _subjects = context.Set<Subject>();
        }
        #endregion

        #region Methods
        #endregion
    }
}
