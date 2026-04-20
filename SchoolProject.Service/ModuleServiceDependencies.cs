using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Service.Abstracts;
using SchoolProject.Service.Abstracts.Views;
using SchoolProject.Service.BackgroundServices;
using SchoolProject.Service.Implementations;
using SchoolProject.Service.Implementations.Views;

namespace SchoolProject.Service;

public static class ModuleServiceDependencies
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    {
        // Register your Services here
        // "Scoped" is usually best for Services (Created once per request)
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IApplicationUserService, ApplicationUserService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IViewService, ViewService>();
        services.AddScoped<IInstructorService, InstructorService>();
        //services.AddScoped<IFileService,FileService>();

        // Register the RefreshTokenCleanupService as a Hosted Service (Background Service)
        services.AddHostedService<RefreshTokenCleanupService>();

        // If you have more services later, add them here:
        // services.AddScoped<ITeacherService, TeacherService>();

        return services;
    }

}
