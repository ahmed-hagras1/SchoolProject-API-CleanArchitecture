using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<
        User,                           // 1. TUser: Your custom user class
        IdentityRole<int>,              // 2. TRole: The role class with an int key
        int,                            // 3. TKey: The primary key type for all tables
        IdentityUserClaim<int>,         // 4. TUserClaim
        IdentityUserRole<int>,          // 5. TUserRole
        IdentityUserLogin<int>,         // 6. TUserLogin
        IdentityRoleClaim<int>,         // 7. TRoleClaim
        IdentityUserToken<int>          // 8. TUserToken
    >
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<DepartmentSubject> DepartmentSubjects { get; set; }
    public DbSet<StudentSubject> StudentSubjects { get; set; }
    public DbSet<User> Users { get; set; }
    
    
    // Fluent API.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Department Entity Configuration using Fluent API
        // Use this way to organize configurations instead of Data Annotations.
        // modelBuilder.Entity<Department>(entity =>
        // {
        //     entity.HasKey(d => d.DeptId); // Primary Key
        //     entity.Property(d => d.DeptName).HasMaxLength(200); // DeptName max length
        // });

        modelBuilder.Entity<DepartmentSubject>()
            .HasKey(ds => new { ds.DeptId, ds.SubjectId }); // Composite primary key

        modelBuilder.Entity<StudentSubject>()
            .HasKey(ss => new { ss.StudentId, ss.SubjectId }); // Composite primary key

        modelBuilder.Entity<InstructorSubject>()
            .HasKey(x => new { x.InstructorId, x.SubjectId }); // Composite primary key

        // Circular Reference Handling (Optional but Recommended)
        // When deleting a Department, avoid Cascade Delete on Instructor to prevent cycles
        modelBuilder.Entity<Instructor>()
            .HasOne(i => i.Department)
            .WithMany(d => d.Instructors)
            .HasForeignKey(i => i.DeptId)
            .OnDelete(DeleteBehavior.Restrict);

        //modelBuilder.Entity<Student>()
        //    .Property(s => s.Name)
        //    .HasMaxLength(200);

        // This will automatically apply all configurations from the current assembly, including DepartmentConfigurations.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 

    }
}
