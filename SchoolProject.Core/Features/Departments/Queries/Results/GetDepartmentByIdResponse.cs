using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Departments.Queries.Results
{
    public class GetDepartmentByIdResponse
    {
        public string DepartmentName { get; set; }
        public string ManagerName { get; set; }
        public List<StudentResponse> StudentList { get; set; } = new List<StudentResponse>();
        public List<SubjectResponse> SubjectList { get; set; } = new List<SubjectResponse>();
        public List<InstructorResponse> InstructorList { get; set; } = new List<InstructorResponse>();
    }
    public class StudentResponse
    {
        public string StudentName { get; set; }
        public StudentResponse(string studentName)
        {
            StudentName = studentName;
        }
    }
    public class SubjectResponse
    {
        public string SubjectName { get; set; }
    }
    public class InstructorResponse
    {
        public string InstructorName { get; set; }
    }
}
