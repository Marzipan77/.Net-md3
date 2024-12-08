using MD3Marcis.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MD3Marcis.Model
{
    public class Course
    {
        // Atsauce 9. Lekcija
        [Key]
        public int Id { get; set; } // Primary key
        public string Name { get; set; }

        [ForeignKey("Teacher")]
        public int TeacherId { get; set; } // Foreign key
        public Teacher Teacher { get; set; }

        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>(); // 1:N Relation
    }
}
