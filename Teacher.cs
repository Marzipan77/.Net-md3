using MD3Marcis.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MD3Marcis.Model
{
    public class Teacher : Person
    {
        public DateTime ContractDate { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>(); // 1:N Relation
    }
}
