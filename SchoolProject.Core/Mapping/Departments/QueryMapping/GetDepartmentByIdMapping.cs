using SchoolProject.Core.Features.Departments.Queries.Results;
using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Mapping.Departments
{
    public partial class DepartmentProfile
    {
        public void GetDepartmentByIdMapping()
        {
            CreateMap<Department, GetDepartmentByIdResponse>()
                .ForMember(destination => destination.DepartmentName, opt => opt.MapFrom(src => src.DeptName))
                .ForMember(destination => destination.ManagerName, opt => opt.MapFrom(src => src.InstructorManager != null ? src.InstructorManager.Name : string.Empty))
                .ForMember(destination => destination.InstructorList, opt => opt.MapFrom(src => src.Instructors))
                .ForMember(destination => destination.SubjectList, opt => opt.MapFrom(src => src.DepartmentSubjects));

            CreateMap<Instructor, InstructorResponse>()
                .ForMember(destination => destination.InstructorName, opt => opt.MapFrom(src => src.Name));

            CreateMap<DepartmentSubject, SubjectResponse>()
                .ForMember(destination => destination.SubjectName, opt => opt.MapFrom(src => src.Subject.SubjectName));
        }
    }
}
