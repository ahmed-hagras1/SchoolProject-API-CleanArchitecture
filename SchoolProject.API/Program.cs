
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using SchoolProject.API.MiddleWares;
using SchoolProject.Core;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.Dependencies;
using SchoolProject.Service;
using System.Globalization;

namespace SchoolProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Localization
            // 1. Add Localization Service
            builder.Services.AddLocalization(options => options.ResourcesPath = "");
            #endregion

            // Configure Database Connection String
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            //});

            #region Dependency injection.
            // 🟢 THIS IS THE BEST POSITION
            // Just call your custom extension method.
            // It will register all infrastructure dependencies
            // service dependencies at once.
            // and core dependencies.
            builder.Services.AddInfrastructureDependencies(builder.Configuration)
                .AddServiceDependencies()
                .AddCoreDependencies()
                .AddIdentityDependencies(builder.Configuration);
            #endregion

            #region CORS Configuration
            // 1. Add CORS services to the container
            // We create a policy named "AllowAll"
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins()
                          .AllowAnyHeader() 
                          .AllowAnyMethod();
                });
            });
            #endregion

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            var app = builder.Build();

            // 2. Apply the CORS middleware
            // VERY IMPORTANT: This must go BEFORE app.UseAuthorization() and app.MapControllers()
            app.UseCors("AllowSpecificOrigins");
            
            
            // Use Custom MiddleWares
            app.UseMiddleware<ErrorHandlerMiddleware>();

            #region Localization Middleware
            // 2. Configure Supported Cultures
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("ar-EG") // Add more if needed
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ar-EG"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
