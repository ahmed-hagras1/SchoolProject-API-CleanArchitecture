
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SchoolProject.API.MiddleWares;
using SchoolProject.Core;
using SchoolProject.Core.Filters;
using SchoolProject.Data.Entities.Identity;
using SchoolProject.Infrastructure.Data;
using SchoolProject.Infrastructure.Dependencies;
using SchoolProject.Infrastructure.Seeder;
using SchoolProject.Service;
using System.Globalization;

namespace SchoolProject.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ==========================================
            // 🟢 2. SERILOG CONFIGURATION
            // ==========================================
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog(); // Tell ASP.NET Core to use Serilog

            try
            {
                Log.Information("Starting the Web API application...");

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
                        policy.AllowAnyOrigin()
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

                // Add Custom Filters
                builder.Services.AddScoped<AuthenticationFilter>();


                var app = builder.Build();

                // ==========================================
                // 🟢 4. SERILOG REQUEST LOGGING MIDDLEWARE
                // ==========================================
                // Place this early in the pipeline to log every request!
                app.UseSerilogRequestLogging();

                app.UseStaticFiles(); // 🟢 Allows serving files from the wwwroot folder

                // ==========================================
                // 🟢 SEED DATABASE DATA
                // ==========================================
                using (var scope = app.Services.CreateScope())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>(); // Or IdentityRole<int> depending on your exact setup

                    // 1. Seed Roles First!
                    await RoleSeeder.SeedAsync(roleManager);

                    // 2. Seed Default User Second!
                    await UserSeeder.SeedAsync(userManager);
                }

                // 2. Apply the CORS middleware
                // VERY IMPORTANT: This must go BEFORE app.UseAuthorization() and app.MapControllers()
                app.UseCors("AllowAll");


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


                app.UseAuthentication();
                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                // 🟢 6. CATCH FATAL ERRORS WITH SERILOG
                Log.Fatal(ex, "The application failed to start correctly");
            }
            finally
            {
                // 🟢 7. FLUSH LOGS ON SHUTDOWN
                Log.CloseAndFlush();
            }
        }
    }
}
