using Microsoft.Extensions.Logging;
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
        // 1. Inject standard ILogger
        private readonly ILogger<InstructorService> _logger;

        // 1. Inject the Infrastructure Repository
        public InstructorService(IInstructorRepository instructorRepository, ILogger<InstructorService> logger)
        {
            _instructorRepository = instructorRepository;
            _logger = logger;
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
                _logger.LogInformation("Attempting to add a new instructor named {InstructorName}...", instructor.Name);

                // Use the generic repository to save the entity
                await _instructorRepository.AddAsync(instructor);

                _logger.LogInformation("Successfully added instructor {InstructorName} to the database.", instructor.Name);

                return "Success";
            }
            catch (Exception ex)
            {
                // This will log the entire stack trace to the SQL Database!
                _logger.LogError(ex, "A critical error occurred while adding instructor {InstructorName}.", instructor.Name);

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