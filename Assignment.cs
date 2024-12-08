using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MD3Marcis.Model
{
    public class Assignment
    {
        // Atsauce 9. Lekcija
        [Key]
        public int Id { get; set; } // Primary key
        public DateTime Deadline { get; set; }
        public string Description { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; } 
        public Course Course { get; set; }

        public ICollection<Submission> Submissions { get; set; } = new List<Submission>(); // 1:N relation
    }
}
