using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Departments.Queries.Models;
using SchoolProject.Core.Features.Departments.Queries.Results;
using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using SchoolProject.Service.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Departments.Queries.Handlers
{
    public class DepartmentQueryHandler : ResponseHandler,
        IRequestHandler<GetDepartmentListQuery, Response<List<GetDepartmentListResponse>>>,
        IRequestHandler<GetDepartmentByIdQuery, Response<GetDepartmentByIdResponse>>
    {

        #region Fields
        private readonly IDepartmentService _departmentService;
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructor
        public DepartmentQueryHandler(IDepartmentService departmentService,
            IStudentService studentService,
            IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _departmentService = departmentService;
            _studentService = studentService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Method
        public async Task<Response<List<GetDepartmentListResponse>>> Handle(GetDepartmentListQuery request, CancellationToken cancellationToken)
        {
            var departmentList = await _departmentService.GetDepartmentsListAsync();

            var departmentListMapper = _mapper.Map<List<GetDepartmentListResponse>>(departmentList);

            return Success(departmentListMapper, new { Count = departmentListMapper.Count });
        }

        public async Task<Response<GetDepartmentByIdResponse>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
        {
            var department = await _departmentService.GetDepartmentsByIdWithIncludeAsync(request.Id);

            if (department == null) return NotFound<GetDepartmentByIdResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);
            // Map Department to GetDepartmentByIdResponse
            var departmentMapper = _mapper.Map<GetDepartmentByIdResponse>(department);

            // Paginate the StudentList.
            Expression<Func<Student, StudentResponse>> expression =
                e => new StudentResponse(e.Name);

            var queryable = _studentService.FilterStudentsByDepartmentIdPaginatedQueryable(request.Id,search: (string.IsNullOrEmpty(request.StudentSearch) ? null : request.StudentSearch.Trim()), request.StudentOrderBy);

            var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.StudentPageNumber, request.StudentPageSize);
            departmentMapper.StudentList = paginatedList.Data.ToList();

            return Success(departmentMapper, 
                new { RetrivedAt = DateTime.UtcNow, StudentsCount = departmentMapper.StudentList.Count, InstructorsCount = departmentMapper.InstructorList.Count, SubjectsCount = departmentMapper.SubjectList.Count  });
        }
        #endregion
    }
}
