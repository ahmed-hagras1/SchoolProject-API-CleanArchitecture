using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Abstracts
{
    public interface IStudentService
    {
        public Task<List<Student>> GetStudentsListAsync();
        public Task<Student> GetStudentByIdWithIncludeAsync(int studentId);
        public Task<Student> GetStudentByIdAsync(int studentId);
        public Task<string> CreateStudentAsync(Student student);
        public Task<bool> IsNameExistExcludeSelf(string name, int id = 0);
        public Task<bool> IsStudentIdExist(int id);
        public Task<string> UpdateStudentAsync(Student student);
        public Task<string> DeleteStudentAsync(Student student);
    }
}
