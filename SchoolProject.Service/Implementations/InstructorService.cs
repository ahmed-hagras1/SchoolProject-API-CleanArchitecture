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
        private readonly IInstructorRepository _instructorRepository;

        // 1. Inject the Infrastructure Repository
        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        // ==========================================
        // 🟢 Add New Instructor
        // ==========================================
        public async Task<string> AddInstructorAsync(Instructor instructor)
        {
            // Note: The file upload is handled cleanly in the MediatR handler.
            // By the time the entity gets here, the 'Image' property already holds the string path!

            try
            {
                // Use the generic repository to save the entity
                await _instructorRepository.AddAsync(instructor);
                return "Success";
            }
            catch (Exception)
            {
                // In an enterprise app, you might log the exception here
                return "Failed";
            }
        }

        // ==========================================
        // 🟢 Update Salary via Stored Procedure
        // ==========================================
        public async Task<string> UpdateSalaryProcedureAsync(int instructorId, decimal newSalary)
        {
            // Call the custom repository method that executes the raw SQL
            var rowsAffected = await _instructorRepository.UpdateInstructorSalaryAsync(instructorId, newSalary);

            // If at least one row was updated, it was successful
            if (rowsAffected > 0)
            {
                return "Success";
            }

            return "Failed";
        }
    }
}