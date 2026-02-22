using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Core.AppMetaData
{
    public static class Router
    {
        // Define root constants
        public const string SignleRoute = "/{id}";
        public const string root = "Api";
        public const string version = "V1";
        public const string Rule = root + "/" + version + "/";

        public static class StudentRouting
        {
            public const string Prefix = Rule + "Student";
            public const string List = Prefix + "/List";
            public const string GetById = Prefix + SignleRoute;
            // Result: "Api/V1/Student/{id}"
            public const string Add = Prefix + "/Add";
            public const string Update = Prefix + "/Update";
            public const string Delete = Prefix + "/Delete" + SignleRoute;
            public const string Paginated = Prefix + "/Paginated";
        }

        public static class DepartmentRouting
        {
            public const string Prefix = Rule + "Department";
            public const string List = Prefix + "/List";
            public const string GetById = Prefix + "/GetById";
        }
        public static class ApplicationUserRouting
        {
            public const string Prefix = Rule + "ApplicationUser";
            public const string Add = Prefix + "/Add";
            public const string GetPaginatedList = Prefix + "/GetPaginatedList";
            public const string GetUserById = Prefix + SignleRoute;
            public const string Update = Prefix + "/Update";
        }
    }
}
