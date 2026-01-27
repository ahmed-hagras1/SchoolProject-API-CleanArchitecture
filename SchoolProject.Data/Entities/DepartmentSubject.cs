using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.Entities;

public class DepartmentSubject
{
    public int DeptId { get; set; }
    public int SubjectId { get; set; }
    // Navigation Properties
    [ForeignKey("DeptId")]
    public virtual Department Department { get; set; }
    [ForeignKey("SubjectId")]
    public virtual Subject Subject { get; set; }
}
