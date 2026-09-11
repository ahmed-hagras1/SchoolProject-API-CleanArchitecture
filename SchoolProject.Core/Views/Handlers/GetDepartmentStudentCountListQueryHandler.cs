using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Data.Entities.Views;
using SchoolProject.Service.Abstracts.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolProject.Core.Features.Views.Queries.Models; // Add this!

namespace SchoolProject.Core.Features.Views.Handlers
{
    public class GetDepartmentStudentCountListQueryHandler : ResponseHandler,
        IRequestHandler<GetDepartmentStudentCountListQuery, Response<List<ViewDepartmentStudentCount>>>
    {
        private readonly IViewService _viewService;

        // Inject your IViewService and the Localizer for your ResponseHandler
        public GetDepartmentStudentCountListQueryHandler(
            IViewService viewService,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _viewService = viewService;
        }

        public async Task<Response<List<ViewDepartmentStudentCount>>> Handle(GetDepartmentStudentCountListQuery request, CancellationToken cancellationToken)
        {
            // 1. Get the data from the Service
            var result = await _viewService.GetDepartmentStudentCountListAsync();

            // 2. Return the standard Success response
            return Success(result);
        }
    }
}
