using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.Entities.Views
{
    public class ViewDepartmentStudentCount
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public int StudentsInDepartment { get; set; }
    }
}
