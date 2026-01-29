using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Abstracts
{
    public interface IStudentRepository : IGenericRepositoryAsync<Student>
    {
        public Task<List<Student>> GetStudentsListAsync();
        public Task<Student> GetStudentByIdWithIncludeAsync(int studentId);
        public Task<Student> GetStudentByIdAsync(int studentId);
        Task<bool> IsNameExistExcludeSelf(string name, int id = 0);
        Task<bool> IsStudentIdExist(int studentId);

    }
}
