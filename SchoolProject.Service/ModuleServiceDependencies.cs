using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Service.Abstracts;
using SchoolProject.Service.Implementations;

namespace SchoolProject.Service;

public static class ModuleServiceDependencies
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        // Register your Services here
        // "Scoped" is usually best for Services (Created once per request)
        services.AddScoped<IStudentService, StudentService>();

        // If you have more services later, add them here:
        // services.AddScoped<ITeacherService, TeacherService>();

        return services;
    }

}
