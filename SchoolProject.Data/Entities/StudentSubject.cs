using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.Entities;

public class StudentSubject
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    // Navigation Properties
    [ForeignKey(nameof(StudentId))]
    public virtual Student Student { get; set; }
    [ForeignKey(nameof(SubjectId))]
    [InverseProperty(nameof(Subject.StudentSubjects))]
    public virtual Subject Subject { get; set; }
}
