using AutoMapper;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Mapping.ApplicationUsers
{
    public partial class ApplicationUserProfile
    {
        public void AddApplicationUserCommandMapping()
        {
            CreateMap<AddApplicationUserCommand, User>();
        }
    }
}