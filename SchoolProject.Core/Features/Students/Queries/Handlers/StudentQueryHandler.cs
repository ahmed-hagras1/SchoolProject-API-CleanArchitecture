using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Features.Students.Queries.Handlers
{
    public class StudentQueryHandler :ResponseHandler, 
        IRequestHandler<GetStudentListQuery,Response<List<GetStudentListResponse>>>,
        IRequestHandler<GetStudentByIdQuery, Response<GetStudentResponse>>,
        IRequestHandler<GetStudentPaginatedListQuery, PaginatedResult<GetStudentPaginatedListResponse>>
    {
        #region Fields
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _stringLocalizer;
        #endregion

        #region Constructor
        public StudentQueryHandler(IStudentService studentService,
            IMapper mapper,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _studentService = studentService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Handle Method
        public async Task <Response<List<GetStudentListResponse>>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            var studentsList =  await _studentService.GetStudentsListAsync();
            // Convert List<Student> to List<GetStudentListResponse>
            var studentListMapper = _mapper.Map<List<GetStudentListResponse>>(studentsList);

            return Success(studentListMapper, new {Count = studentListMapper.Count});
        }

        public async Task<Response<GetStudentResponse>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _studentService.GetStudentByIdWithIncludeAsync(request.StudentId);

            if (student == null) return NotFound<GetStudentResponse>(_stringLocalizer[SharedResourcesKeys.NotFound]);

            var studentMapper = _mapper.Map<GetStudentResponse>(student);
            return Success(studentMapper, new { RetrivedAt = DateTime.UtcNow, Source = "Database"});
        }

        //public Task<PaginatedResult<GetStudentPaginatedListResponse>> Handle(GetStudentPaginatedListQuery request, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}
        public async Task<PaginatedResult<GetStudentPaginatedListResponse>> Handle(GetStudentPaginatedListQuery request, CancellationToken cancellationToken)
        {


            Expression<Func<Student, GetStudentPaginatedListResponse>> expression = 
                e => new GetStudentPaginatedListResponse(e.StudId, e.Name, e.Address, e.Department.DeptName);

            var queryable = _studentService.FilterStudentsPaginatedQueryable(search: (string.IsNullOrEmpty(request.Search)? null : request.Search.Trim()) , request.OrderBy);
   
            var paginatedList = await queryable.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return paginatedList;
        }
        #endregion
    }
}
