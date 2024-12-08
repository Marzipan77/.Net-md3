using MD3Marcis.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Tikai izmantojot 9. lekcijas dbContextCourses tiku garām  error, kas katru reizi metās kad centos initial create palaist
// Tāpēc izmeiģināju paņemt jūsu failu īsti nesapratu kas mainījāš bet nostrādāja un paliku pie tā

namespace MD3Marcis.Model
{
    // Database konteksta class, forEntity Framework Core 
    public class dbContextCourses : DbContext
    {
        // DbSet īpašības, kas atbilst tabulām datubāzē
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        //  9. Lekcija
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string cs = ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            optionsBuilder.UseSqlServer(cs);
        }

        // Atsauce (https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding)
  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure the base method is called

            // Starting data for teachers
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher { Id = 1, Name = "Laura", Surname = "Kļaviņa", Gender = Gender.Woman, ContractDate = new DateTime(2019, 5, 10) },
                new Teacher { Id = 2, Name = "Mārtiņš", Surname = "Rozītis", Gender = Gender.Man, ContractDate = new DateTime(2021, 8, 1) }
            );

            // Starting data for courses
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Programmēšana", TeacherId = 1 }, // Assigned to Laura
                new Course { Id = 2, Name = "Datu bāzes", TeacherId = 2 } // Assigned to Mārtiņš
            );

            // Starting data for students
            modelBuilder.Entity<Student>().HasData(
              new Student { Id = 1, Name = "Elza", Surname = "Ziediņa", Gender = Gender.Woman, StudentIdNumber = 3001 },
              new Student { Id = 2, Name = "Artis", Surname = "Balodis", Gender = Gender.Man, StudentIdNumber = 3002 },
              new Student { Id = 3, Name = "Rūta", Surname = "Lāce", Gender = Gender.Woman, StudentIdNumber = 3003 },
              new Student { Id = 4, Name = "Edgars", Surname = "Bērziņš", Gender = Gender.Man, StudentIdNumber = 3004 }
);

            // Starting data for assignments
            modelBuilder.Entity<Assignment>().HasData(
                new Assignment { Id = 1, Deadline = new DateTime(2024, 3, 1), CourseId = 1, Description = "Python" },
                new Assignment { Id = 2, Deadline = new DateTime(2024, 4, 15), CourseId = 2, Description = "SQL skripti" }
            );

            // Starting data for submissions

            modelBuilder.Entity<Submission>().HasData(
    new Submission { Id = 3, AssignmentId = 1, StudentId = 1, SubmissionTime = new DateTime(2024, 3, 1, 10, 30, 0), Score = 90 },
    new Submission { Id = 4, AssignmentId = 1, StudentId = 3, SubmissionTime = new DateTime(2024, 3, 1, 12, 45, 0), Score = 75 },
    new Submission { Id = 5, AssignmentId = 2, StudentId = 2, SubmissionTime = new DateTime(2024, 4, 15, 11, 15, 0), Score = 85 },
    new Submission { Id = 6, AssignmentId = 2, StudentId = 4, SubmissionTime = new DateTime(2024, 4, 15, 14, 20, 0), Score = 80 }
);
        }
    }
}
