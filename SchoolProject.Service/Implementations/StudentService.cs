using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            //var student = await _studentRepository.GetByIdAsync(studentId);
            var student = await _studentRepository.GetStudentByIdWithIncludeAsync(studentId);

            return student;
        }

        public async Task<string> CreateStudentAsync(Student student)
        {
            // Check if the name is exists
            var isNameExist = await _studentRepository.IsNameExist(student.Name);

            if (isNameExist)
                return "Exist"; // Return specific status

            // Add the student
            await _studentRepository.AddAsync(student);

            // Return Success
            return "Success";
        }

        public Task<bool> IsNameExist(string name)
        {
            return _studentRepository.IsNameExist(name);
        }
        #endregion
    }
}