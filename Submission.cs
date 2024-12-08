using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MD3Marcis.Model
{
    public class Submission
    {
        // Atsauce 9. Lekcija
        [Key]
        public int Id { get; set; } // Primary key
        public DateTime SubmissionTime { get; set; }
        public int Score { get; set; }

        [ForeignKey("Assignment")]
        public int AssignmentId { get; set; } // Foreign key 
        public Assignment Assignment { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; } // Foreign key
        public Student Student { get; set; }

    

        [NotMapped]  // Indicates that this property should not be mapped to a database column
        public string DisplayText
        {
            get
            {
                // Determines the student's full name
                // If no student is associated with the submission, "Unknown Student" is used
                var studentName = Student != null ? $"{Student.FullName}" : "Unknown Student";

                // Determines the assignment description
                // If no assignment is associated with the submission, "No Assignment" is used
                var assignmentDesc = Assignment != null ? Assignment.Description : "No Assignment";
                // Returns a formatted string that includes the student's name, 
                // assignment description, and the score they achieved
                return $"Student: {studentName}, Assignment: {assignmentDesc}, Score: {Score}";
            }
        }

    }

}
