using SchoolProject.Core.Features.Authorization.Queries.Results;
using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Mapping.Roles
{
    public partial class RoleProfile
    {
        public void GetRolesListMapping()
        {
            // Mapping configuration between Student entity and GetStudentListResponse DTO.
            // Maps DepartmentName from the related Department entity.
            // Maps only DepartmentName because other properties have the same names in both classes Student and GetStudentListResponse.
            CreateMap<Role, GetRolesListResult>()
                .ForMember(destination => destination.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}
