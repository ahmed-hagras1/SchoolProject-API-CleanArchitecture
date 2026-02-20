using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.InfrastructureBases;
using SchoolProject.Infrastructure.Repositories;
using SchoolProject.Service.Abstracts;

namespace SchoolProject.Infrastructure.Dependencies;

public static class ModuleInfrastructureDependencies
{
    // This method registers all infrastructure layer dependencies
    // Extension method for IServiceCollection
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure Database Connection String
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        // Register your infrastructure services here
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IInstructorRepository, InstructorRepository>();

        // Register Generic Repository
        services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));

        return services;
    }
}
