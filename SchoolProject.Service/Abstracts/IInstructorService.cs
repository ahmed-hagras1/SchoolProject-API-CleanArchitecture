using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Abstracts
{
    public interface IInstructorService
    {
        Task<string> UpdateSalaryProcedureAsync(int instructorId, decimal newSalary);
    }
}
