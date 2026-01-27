using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.Entities;

public class Department
{
    public Department()
    {
        Students = new HashSet<Student>();
        DepartmentSubjects = new HashSet<DepartmentSubject>();
    }
    [Key]
    public int DeptId { get; set; }
    [StringLength(200)]
    public string DeptName { get; set; }
    // Navigation Properties
    public virtual ICollection<Student> Students { get; set; }
    public virtual ICollection<DepartmentSubject>DepartmentSubjects { get; set; }
}
