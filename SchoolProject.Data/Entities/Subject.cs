using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.Entities;

public class Subject
{
    public Subject()
    {
        DepartmentSubjects = new HashSet<DepartmentSubject>();
        StudentSubjects = new HashSet<StudentSubject>();
        InstructorSubjects = new HashSet<InstructorSubject>();
    }

    [Key]
    public int SubjectId { get; set; }

    [StringLength(200)]
    public string? SubjectName { get; set; }

    public int? Period { get; set; }

    // Navigation Properties
    [InverseProperty("Subject")]
    public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    [InverseProperty("Subject")]
    public virtual ICollection<DepartmentSubject> DepartmentSubjects { get; set; }

    // Linked the InstructorSubject table
    [InverseProperty("Subject")]
    public virtual ICollection<InstructorSubject> InstructorSubjects { get; set; }
}
