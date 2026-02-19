using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.Mapping.ApplicationUsers
{
    public partial class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            // Application User Mapping.
            AddApplicationUserCommandMapping();
        }
    }
}
