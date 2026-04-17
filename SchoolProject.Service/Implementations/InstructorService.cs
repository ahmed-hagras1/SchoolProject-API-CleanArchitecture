using SchoolProject.Data.Entities;
using SchoolProject.Service.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Service.Implementations
{
    public class InstructorService : IInstructorService
    {
        #region Fields
        private readonly IInstructorRepository _InstructorRepository;
        #endregion

        #region Constructor
        public InstructorService(IInstructorRepository instructorRepository)
        {
            _InstructorRepository = instructorRepository;
        }

        #endregion

        #region Methods / Handle functions

        public async Task<string> UpdateSalaryProcedureAsync(int instructorId, decimal newSalary)
        {
            var rowsAffected = await _InstructorRepository.UpdateInstructorSalaryAsync(instructorId, newSalary);

            if (rowsAffected > 0)
                return "Success";

            return "Failed";
        }
        #endregion
    }
}
