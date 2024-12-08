using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MD3Marcis.Model
{
    public class Student : Person
    {
        public int StudentIdNumber { get; set; }
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>(); // 1:N Relation
    }
}
