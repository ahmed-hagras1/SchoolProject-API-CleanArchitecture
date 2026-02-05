using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.Entities
{
    public class InstructorSubject
    {
        public int InstructorId { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey(nameof(InstructorId))]
        public virtual Instructor Instructor { get; set; }
        [ForeignKey(nameof(SubjectId))]
        [InverseProperty(nameof(Subject.InstructorSubjects))]
        public virtual Subject Subject { get; set; }
    }
}
