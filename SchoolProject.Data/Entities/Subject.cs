using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
    [Key]
    public int SubjectId { get; set; }
    [StringLength(200)]
    public string SubjectName { get; set; }
    public DateTime Period { get; set; }
    // Navigation Properties
    public virtual ICollection<StudentSubject> StudentSubjects { get; set; }
    // Navigation Properties
    public virtual ICollection<DepartmentSubject> DepartmentSubjects { get; set; }
}
