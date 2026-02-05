using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.Entities
{
    public class Instructor
    {
        public Instructor()
        {
            InstructorsSupervised = new HashSet<Instructor>();
            InstructorSubjects = new HashSet<InstructorSubject>();
        }

        [Key]
        public int InstructorId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public int? SupervisorId { get; set; }
        public decimal Salary { get; set; }
        public int? DeptId { get; set; }
        // Navigation Properties.
        [ForeignKey(nameof(DeptId))]
        [InverseProperty(nameof(Department.Instructors))]
        public virtual Department? Department { get; set; }

        [InverseProperty(nameof(Department.InstructorManager))]
        public virtual Department? ManagedDepartment { get; set; }

        [ForeignKey(nameof(SupervisorId))]
        [InverseProperty(nameof(InstructorsSupervised))]
        public virtual Instructor? Supervisor { get; set; }

        // Inverse Relationship for Supervisor (One Supervisor has many Instructors)
        [InverseProperty(nameof(Supervisor))]
        public virtual ICollection<Instructor> InstructorsSupervised { get; set; }

        // Many-to-Many relationship with Subject
        public virtual ICollection<InstructorSubject> InstructorSubjects { get; set; }

    }
}
