using MD3Marcis.Model;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MD3Marcis.Model
{
    // Atsauce 9. Lekcija
    public abstract class Person
    {
        [Key]
        public int Id { get; set; } // Primary key
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName => $"{Name} {Surname}";
        public Gender Gender { get; set; }
    }
}
