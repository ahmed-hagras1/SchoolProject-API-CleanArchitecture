using SchoolProject.Data.Entities;
using SchoolProject.Data.Helpers;
using SchoolProject.Service.Abstracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SchoolProject.Service.Implementations
{
    public class StudentService : IStudentService
    {
        #region Fields
        // 1. Define a private field to hold the dependency
        private readonly IStudentRepository _studentRepository;
        #endregion

        #region Constructor
        // 2. Inject the Interface in the Constructor (Dependency Injection)
        // Notice we give the parameter a name: "studentRepository"
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        #endregion

        #region Methods / Handle functions
        // 3. Implement the method
        public async Task<List<Student>> GetStudentsListAsync()
        {
            // Use the field (_studentRepository) to get data
            return await _studentRepository.GetStudentsListAsync();
        }
        public IQueryable<Student> GetStudentsQueryable()
        {
            return _studentRepository.GetStudentsQueryable();
        }
        public IQueryable<Student> FilterStudentsPaginatedQueryable(string search, StudentOrderingEnum orderBy = StudentOrderingEnum.StudentId)
        {
            return _studentRepository.FilterStudentsPaginatedQueryable(search, orderBy);
        }
        public async Task<Student> GetStudentByIdWithIncludeAsync(int studentId)
        {
            //var student = await _studentRepository.GetByIdAsync(studentId);
            var student = await _studentRepository.GetStudentByIdWithIncludeAsync(studentId);

            return student;
        }
        public Task<Student> GetStudentByIdAsync(int studentId)
        {
            return _studentRepository.GetStudentByIdAsync(studentId);
        }

        public async Task<string> CreateStudentAsync(Student student)
        {
            // Add the student
            await _studentRepository.AddAsync(student);

            // Return Success
            return "Success";
        }

        public async Task<bool> IsNameExistExcludeSelf(string name, int id = 0)
        {
            return await _studentRepository.IsNameExistExcludeSelf(name, id);
        }

        public async Task<bool> IsStudentIdExist(int id)
        {
            return await _studentRepository.IsStudentIdExist(id);
        }

        public async Task<string> UpdateStudentAsync(Student student)
        {
            await _studentRepository.UpdateAsync(student);
            return "Success";
        }

        public async Task<string> DeleteStudentAsync(Student student)
        {
            
            await _studentRepository.DeleteAsync(student);
            return "Success";
        }

        public IQueryable<Student> FilterStudentsByDepartmentIdPaginatedQueryable(int departmentId, string search = null, StudentOrderingEnum orderBy = StudentOrderingEnum.StudentId)
        {
            return _studentRepository.FilterStudentsByDepartmentIdPaginatedQueryable(departmentId, search, orderBy);
        }




        #endregion
    }
}