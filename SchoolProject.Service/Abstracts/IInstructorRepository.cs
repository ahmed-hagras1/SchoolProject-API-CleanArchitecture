using SchoolProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Abstracts;

public interface IInstructorRepository: IGenericRepositoryAsync<Instructor>
{
    Task<int> UpdateInstructorSalaryAsync(int instructorId, decimal newSalary);
}
