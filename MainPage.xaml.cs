using MD3Marcis.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MD3Marcis
{

    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> DataItems { get; set; }

        public MainPage()
        {
            InitializeComponent();
            DataItems = new ObservableCollection<string>();
            DataView.ItemsSource = DataItems;

            LoadCoursesIntoPicker();
            LoadStudentsIntoPicker();
            LoadAssignmentsIntoPicker();
            LoadSubmissionsIntoPicker();
        }

        // Data recieving and displaying
        private void FetchAndDisplayData(Func<dbContextCourses, List<string>> fetchData)
        {
            try
            {
                DataItems.Clear(); // data clear
                using (var context = new dbContextCourses())
                {
                    var data = fetchData(context); // Gets data, with fetch function
                    foreach (var item in data)
                    {
                        DataItems.Add(item); // adds each data elements for viewing
                    }
                }
            }
            catch (Exception ex)// Handle errors
            {
                DisplayAlert("Error", $"Error loading data: {ex.Message}", "OK"); // Show error message
            }
        }

        // method for student display
        private void ShowStudents(object sender, EventArgs e)
        {
            // shows student data from student data base
            FetchAndDisplayData(context => context.Students
                .Select(s => $"Student ID: {s.StudentIdNumber}, Name: {s.FullName}, Gender: {s.Gender}").ToList());
            ToggleForms("Student"); // Show student-related form
        }

        // method for course display
        private void ShowCourses(object sender, EventArgs e)
        {
            FetchAndDisplayData(context => context.Courses
                .Include(c => c.Teacher) // Include teacher data
                .Select(c => $"Course: {c.Name}, Teacher: {c.Teacher.FullName}")// Fetch course data
                .ToList());
            ToggleForms("");// No specific form toggled
        }

        // method for Assignment display
        private void ShowAssignments(object sender, EventArgs e)
        {
            FetchAndDisplayData(context => context.Assignments
                .Select(a => $"Course: {a.Course.Name}, Description: {a.Description}, Deadline: {a.Deadline.ToShortDateString()}").ToList());
            ToggleForms("Assignment"); // Show assignment-related form
        }

        // method for Submmision display
        private void ShowSubmissions(object sender, EventArgs e)
        {
            FetchAndDisplayData(context => context.Submissions
                .Select(s => $"Student: {s.Student.FullName}, Assignment: {s.Assignment.Description}, Score: {s.Score}").ToList());
            ToggleForms("Submission");// Show submission-related form
        }

        // method for Teachers display
        private void ShowTeachers(object sender, EventArgs e)
        {
            FetchAndDisplayData(context => context.Teachers
                .Select(t => $"Teacher: {t.FullName}, Contract Date: {t.ContractDate.ToShortDateString()}")
                .ToList());
            ToggleForms("");
        }

        // Toggle visibility of forms based on the type
        private void ToggleForms(string formType)
        {
            StudentCrudForm.IsVisible = formType == "Student";
            AssignmentCrudForm.IsVisible = formType == "Assignment";
            SubmissionCrudForm.IsVisible = formType == "Submission";
        }

        // Load course data into a picker
        private void LoadCoursesIntoPicker()
        {
            try
            {
                using (var context = new dbContextCourses())
                {
                    var courses = context.Courses
                        .Select(c => new Course { Id = c.Id, Name = c.Name })
                        .ToList();

                    if (courses.Count == 0) // Check if courses are available
                    {
                        DisplayAlert("Nav datu", "Datubāzē netika atrasts neviens kurss", "Labi");
                        return;
                    }

                    CoursePicker.ItemsSource = courses; 
                    CoursePicker.ItemDisplayBinding = new Binding("Name"); // Display feature 
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("No Data", "No courses found in the database.", "OK"); // Notify no courses found
            }
        }

        // Load student data into pickers
        private void LoadStudentsIntoPicker()
        {
            try
            {
                using (var context = new dbContextCourses())
                {
                    var students = context.Students
                        .Select(s => new Student
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Surname = s.Surname,
                            Gender = s.Gender,
                            StudentIdNumber = s.StudentIdNumber
                        })
                        .ToList();

                    if (students.Count == 0)// Check if students are available
                    {
                        DisplayAlert("No Data", "No students found in the database.", "OK"); // Notify no students found
                        return;
                    }
                    // Assign students to the pickers
                    StudentPicker.ItemsSource = students; // Student data added
                    EditStudentPicker.ItemsSource = students;
                    DeleteStudentPicker.ItemsSource = students;

                    // Set display binding for pickers
                    StudentPicker.ItemDisplayBinding = new Binding("FullName");
                    EditStudentPicker.ItemDisplayBinding = new Binding("FullName");
                    DeleteStudentPicker.ItemDisplayBinding = new Binding("FullName");
                }
            }
            catch (Exception ex)// Handle errors
            {
                DisplayAlert("Error", $"Error loading students: {ex.Message}", "OK"); // Show error message
            }
        }

        private void LoadSubmissionsIntoPicker()
        {
            try
            {
                using (var context = new dbContextCourses()) // Opens data base context
                {
                    // Retrieve submissions along with related student and assignment data
                    var submissions = context.Submissions
                        .Include(s => s.Student) // Include related student data
                        .Include(s => s.Assignment) // Include related assignment data
                        .Select(s => new
                        {
                            Submission = s, // The full submission object
                            DisplayText = $"Student: {s.Student.FullName}, Assignment: {s.Assignment.Description}, Score: {s.Score}"
                        })
                        .ToList(); // Convert the result to a list

                    if (submissions.Count == 0) // Check if any submissions were found
                    {
                        DisplayAlert("Kļūda", "Datubāzē netika atrasts neviens iesniegums.", "Labi"); // Notify if the database contains no data
                        return;
                    }

                    // Assign the retrieved submissions to the pickers
                    EditSubmissionPicker.ItemsSource = submissions.Select(s => s.Submission).ToList(); // Add submissions to the editing picker
                    DeleteSubmissionPicker.ItemsSource = submissions.Select(s => s.Submission).ToList();// Add submissions to the deletion picker

                    foreach (var submission in EditSubmissionPicker.ItemsSource)
                    {
                        Console.WriteLine(submission);   // Output each submission to the console
                    }

                    // Set the display text for the pickers
                    EditSubmissionPicker.ItemDisplayBinding = new Binding("DisplayText"); // Bind display text for the editing picker
                    DeleteSubmissionPicker.ItemDisplayBinding = new Binding("DisplayText"); // Bind display text for the deletion picke
                }
            }
            catch (Exception ex) // Apstrādā kļūdas gadījumā
            {
                DisplayAlert("Error", $"Error loading data: {ex.Message}", "OK"); // Show an error alert
            }
        }

        // Add a new student
        private void AddStudent(object sender, EventArgs e)
        {
            try
            {
                using (var context = new dbContextCourses()) // Open database context
                {
                  
                    var student = new Student
                    {
                        Name = StudentNameEntry.Text, 
                        Surname = StudentSurnameEntry.Text, 
                        Gender = (Gender)Enum.Parse(typeof(Gender), StudentGenderPicker.SelectedItem.ToString()),
                        StudentIdNumber = int.Parse(StudentIdNumberEntry.Text) 
                    };

                    context.Students.Add(student); // Add student to the database
                    context.SaveChanges(); // Save changes to the database
                    DisplayAlert("Success", "Student added successfully!", "OK"); // Notify success

                    LoadStudentsIntoPicker();// Refresh student data in the picker

                    // Clear input fields
                    ClearStudentInputs();

                    // Dynamically update the student data view
                    RefreshStudentData();
                }
            }
            catch (Exception ex) 
            {
                DisplayAlert("Error", $"Error adding student: {ex.Message}", "OK"); // Show error message
            }
        }

        // Edit an existing student
        private void EditStudent(object sender, EventArgs e)
        {
            try
            {
                var selectedStudent = EditStudentPicker.SelectedItem as Student;  // Get selected student from picker
                if (selectedStudent == null) // Check if a student is selected
                {
                    DisplayAlert("Error", "Please select a student to edit.", "OK"); // Notify if no student is selected
                    return; 
                }

                using (var context = new dbContextCourses()) // Open database context
                {
                    var student = context.Students.FirstOrDefault(s => s.Id == selectedStudent.Id); // Find student by ID
                    if (student != null) // Check if student exists
                    {
                        // Update student details from input fields
                        student.Name = EditStudentNameEntry.Text;
                        student.Surname = EditStudentSurnameEntry.Text;
                        student.StudentIdNumber = int.Parse(EditStudentIdNumberEntry.Text);
                        student.Gender = (Gender)Enum.Parse(typeof(Gender), EditStudentGenderPicker.SelectedItem.ToString());
                        context.SaveChanges(); // Save changes to the database
                        DisplayAlert("Success", "Student edited successfully!", "OK"); // Notify success

                        // Clear input fields
                        ClearStudentInputs();

                        // Refresh student data dynamically
                        RefreshStudentData();

                        LoadStudentsIntoPicker(); // Refresh pickers
                    }
                }
            }
            catch (Exception ex) // Handle errors
            {
                DisplayAlert("Error", $"Error editing student: {ex.Message}", "OK"); // Show error message
            }
        }


        // Delete a student
        private void DeleteStudent(object sender, EventArgs e)
        {
            try
            {
                var selectedStudent = DeleteStudentPicker.SelectedItem as Student; // Get selected student from picker
                if (selectedStudent == null) // Check if a student is selected
                {
                    DisplayAlert("Error", "Please select a student to delete.", "OK"); // Notify if no student is selected
                    return; 
                }

                using (var context = new dbContextCourses()) // Open database context
                {
                    var student = context.Students.FirstOrDefault(s => s.Id == selectedStudent.Id); // find student by ID
                    if (student != null) // Check if student exists
                    {
                        context.Students.Remove(student); //Deletes student from database
                        context.SaveChanges(); // saves changes in database
                        DisplayAlert("Success", "Student deleted successfully!", "OK"); // Notify success

                       
                        ClearStudentInputs();// Clear input fields

                        
                        RefreshStudentData();// Refresh student data dynamically

                        LoadStudentsIntoPicker();  // Refresh pickers
                    }
                }
            }
            catch (Exception ex) // Handle errors
            {
                DisplayAlert("Error", $"Error deleting student: {ex.Message}", "OK"); // Show error message
            }
        }

        // Refresh
        private void RefreshStudentData()
        {
            try
            {
                using (var context = new dbContextCourses())
                {
                    // Refresh the student list and display it
                    var students = context.Students
                        .Select(s => $"Student ID: {s.StudentIdNumber}, Name: {s.FullName}, Gender: {s.Gender}")
                        .ToList();

                    DataItems.Clear();
                    foreach (var student in students)
                    {
                        DataItems.Add(student);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error refreshing student data: {ex.Message}", "OK"); // Show error message
            }
        }

        // Clear input fields related to student data
        private void ClearStudentInputs()
        {
            StudentNameEntry.Text = string.Empty;
            StudentSurnameEntry.Text = string.Empty;
            StudentIdNumberEntry.Text = string.Empty;
            StudentGenderPicker.SelectedItem = null;

            EditStudentNameEntry.Text = string.Empty;
            EditStudentSurnameEntry.Text = string.Empty;
            EditStudentIdNumberEntry.Text = string.Empty;
            EditStudentGenderPicker.SelectedItem = null;

            EditStudentPicker.SelectedItem = null;
            DeleteStudentPicker.SelectedItem = null;
        }
        // Load assignments into pickers
        private void LoadAssignmentsIntoPicker()
        {
            try
            {
                using (var context = new dbContextCourses()) // Open database context
                {
                   
                    var assignments = context.Assignments
                        .Select(a => new Assignment
                        {
                            Id = a.Id, // Assignment ID
                            Description = a.Description, // Assignment description
                            Deadline = a.Deadline, // Assignment deadline
                            CourseId = a.CourseId // Related course ID
                        })
                        .ToList();

                   
                    if (assignments.Count == 0)// Check if assignments exist
                    {
                        DisplayAlert("No Data", "No assignments found in the database.", "OK"); // Notify if no assignments are found
                        return;
                    }

                    // Assign assignments to pickers
                    AssignmentPicker.ItemsSource = assignments; 
                    EditAssignmentPicker.ItemsSource = assignments; 
                    DeleteAssignmentPicker.ItemsSource = assignments;

                    // Set display binding for pickers
                    AssignmentPicker.ItemDisplayBinding = new Binding("Description");
                    EditAssignmentPicker.ItemDisplayBinding = new Binding("Description");
                    DeleteAssignmentPicker.ItemDisplayBinding = new Binding("Description");

                    // Assign to submission editing picker
                    EditSubmissionAssignmentPicker.ItemsSource = assignments;
                    EditSubmissionAssignmentPicker.ItemDisplayBinding = new Binding("Description");
                }
            }
            catch (Exception ex) 
            {
                DisplayAlert("Error", $"Error loading assignments: {ex.Message}", "OK"); // Show error message
            }
        }
        // Add a new assignment
        private void AddAssignment(object sender, EventArgs e)
        {
            try
            {
               
                var selectedCourse = CoursePicker.SelectedItem as Course;// Get selected course
                if (selectedCourse == null)// Check if a course is selected
                {
                    DisplayAlert("Error", "Please select a course.", "OK"); // Notify if no course is selected
                    return; 
                }

                using (var context = new dbContextCourses()) // Open database context
                {
                    var assignment = new Assignment
                    {
                        Description = AssignmentDescriptionEntry.Text,  // Retrieve description from input
                        Deadline = AssignmentDeadlinePicker.Date, // Retrieve deadline from input
                        CourseId = selectedCourse.Id // Set course ID from selected course
                    };

                    context.Assignments.Add(assignment); // Add assignment to the database
                    context.SaveChanges(); // Save changes to the database
                    DisplayAlert("Success", "Assignment added successfully!", "OK"); // Notify success


                    LoadCoursesIntoPicker(); // Refresh courses

                    // Clear input fields
                    ClearAssignmentInputs();

                    LoadAssignmentsIntoPicker();// Refresh assignments in pickers

                    RefreshAssignmentData(); // Update displayed assignment data
                }
            }
            catch (Exception ex) // Handle errors
            {
                DisplayAlert("Error", $"Error adding assignment: {ex.Message}", "OK"); // Show error message
            }
        }
        // Edit an existing assignment
        private void EditAssignment(object sender, EventArgs e)
        {
            try
            {
                var selectedAssignment = EditAssignmentPicker.SelectedItem as Assignment;// Get selected assignment
                if (selectedAssignment == null)// Check if an assignment is selected
                {
                    DisplayAlert("Error", "Please select an assignment to edit.", "OK"); // Notify if no assignment is selected
                    return; 
                }

                using (var context = new dbContextCourses())  // Open database context
                {
                    // Atrodi izvēlēto uzdevumu datubāzē
                    var assignment = context.Assignments.FirstOrDefault(a => a.Id == selectedAssignment.Id);
                    if (assignment != null) // Check if assignment exists
                    {
                        // Update assignment details
                        assignment.Description = EditAssignmentDescriptionEntry.Text; // Update description
                        assignment.Deadline = EditAssignmentDeadlinePicker.Date; // Update deadline
                        context.SaveChanges(); // Save changes to the database
                        DisplayAlert("Success", "Assignment edited successfully!", "OK"); // Notify success

                        ClearAssignmentInputs();

                        RefreshAssignmentData(); 

                        LoadAssignmentsIntoPicker(); 
                    }
                }
            }
            catch (Exception ex) 
            {
                DisplayAlert("Error", $"Error editing assignment: {ex.Message}", "OK"); // Show error message
            }
        }
        // Delete an assignment
        private void DeleteAssignment(object sender, EventArgs e)
        {
            try
            {
                var selectedAssignment = DeleteAssignmentPicker.SelectedItem as Assignment;// Get selected assignment
                if (selectedAssignment == null) // Check if an assignment is selected
                {
                    DisplayAlert("Error", "Please select an assignment to delete.", "OK"); // Notify if no assignment is selected
                    return; 
                }

                using (var context = new dbContextCourses())  // Open database context
                {
                    // Atrodi izvēlēto uzdevumu datubāzē
                    var assignment = context.Assignments.FirstOrDefault(a => a.Id == selectedAssignment.Id);// Find assignment by ID
                    if (assignment != null)  // Check if assignment exists
                    {
                        context.Assignments.Remove(assignment); // Remove assignment from database
                        context.SaveChanges(); // Save changes to the database
                        DisplayAlert("Success", "Assignment deleted successfully!", "OK"); // Notify success

                        ClearAssignmentInputs();  // Clear input fields

                        RefreshAssignmentData(); // Refresh assignment data display

                        LoadAssignmentsIntoPicker(); // Refresh pickers
                    }
                }
            }
            catch (Exception ex) // Handle errors
            {
                DisplayAlert("Error", $"Error deleting assignment: {ex.Message}", "OK"); // Show error message
            }
        }

        // Clear input fields related to assignment data
        private void ClearAssignmentInputs()
        {
            AssignmentDescriptionEntry.Text = string.Empty;
            AssignmentDeadlinePicker.Date = DateTime.Today;
            CoursePicker.SelectedItem = null;

            EditAssignmentDescriptionEntry.Text = string.Empty;
            EditAssignmentDeadlinePicker.Date = DateTime.Today;

            EditAssignmentPicker.SelectedItem = null;
            DeleteAssignmentPicker.SelectedItem = null;
        }

        // Refresh the assignment data display
        private void RefreshAssignmentData()
        {
            try
            {
                using (var context = new dbContextCourses())// Open database context
                {
                    var assignments = context.Assignments
                        .Include(a => a.Course)// Include related course data
                        .Select(a => $"Assignment: {a.Description}, Course: {a.Course.Name}, Deadline: {a.Deadline.ToShortDateString()}")
                        .ToList();

                    DataItems.Clear();// Clear existing data
                    foreach (var assignment in assignments)
                    {
                        DataItems.Add(assignment);// Add each assignment to the display
                    }
                }
            }
            catch (Exception ex)// Handle errors
            {
                DisplayAlert("Error", $"Error refreshing assignment data: {ex.Message}", "OK"); // Show error message
            }
        }
        // Add a new submission
        private void AddSubmission(object sender, EventArgs e)
        {
            try
            {
                var selectedStudent = StudentPicker.SelectedItem as Student;// Get selected student
                var selectedAssignment = AssignmentPicker.SelectedItem as Assignment;// Get selected assignment

                // Check if both student and assignment are selected
                if (selectedStudent == null || selectedAssignment == null)
                {
                    DisplayAlert("Error", "Please select both a student and an assignment.", "OK"); // Notify missing selections
                    return; 
                }

                using (var context = new dbContextCourses()) // Open database context
                {
                    var submission = new Submission
                    {
                        StudentId = selectedStudent.Id, // Set student ID
                        AssignmentId = selectedAssignment.Id,// Set assignment ID
                        Score = int.Parse(SubmissionScoreEntry.Text), // Parse score from input
                        SubmissionTime = SubmissionDatePicker.Date // Retrieve submission date from picker
                    };
                    context.Submissions.Add(submission); // Add submission to the database
                    context.SaveChanges(); // Save changes to the database
                    DisplayAlert("Success", "Submission added successfully!", "OK"); // Notify success

                    // Clear input fields
                    ClearSubmissionInputs();

                    
                    LoadSubmissionsIntoPicker();// Refresh submission picker
                    RefreshSubmissionData();// Update displayed submission data
                }
            }
            catch (Exception ex)  // Handle errors
            {
                DisplayAlert("Error", $"Error adding submission: {ex.Message}", "OK"); // Show error message
            }
        }
        // Edit an existing submission
        private void EditSubmission(object sender, EventArgs e)
        {
            try
            {
                var selectedSubmission = EditSubmissionPicker.SelectedItem as Submission;// Get selected submission
                if (selectedSubmission == null) // Check if a submission is selected
                {
                    DisplayAlert("Error", "Please select a submission to edit.", "OK"); // Notify if no submission is selected
                    return;
                }

                var selectedAssignment = EditSubmissionAssignmentPicker.SelectedItem as Assignment;// Get selected submission
                if (selectedAssignment == null) // Check if a submission is selected
                {
                    DisplayAlert("Error", "Please select a submission to edit.", "OK"); // Notify if no submission is selected
                    return; 
                }

                using (var context = new dbContextCourses()) // Open database context
                {
                    var submission = context.Submissions.FirstOrDefault(s => s.Id == selectedSubmission.Id); // Find submission by ID
                    if (submission != null) // Check if submission exists
                    {
                        // Update submission details
                        submission.AssignmentId = selectedAssignment.Id; // Update assignment ID
                        submission.Score = int.Parse(EditSubmissionScoreEntry.Text); // Update score
                        context.SaveChanges(); // Save changes to the database
                        DisplayAlert("Success", "Submission edited successfully!", "OK"); // Notify success

                        LoadAssignmentsIntoPicker();// Refresh assignments in pickers
                        LoadSubmissionsIntoPicker();// Refresh submissions in pickers
                        RefreshSubmissionData();// Update displayed submission data
                        ClearSubmissionInputs();// Clear input fields
                    }
                }
            }
            catch (Exception ex) // Handle errors
            {
                DisplayAlert("Error", $"Error editing submission: {ex.Message}", "OK"); // Show error message
            }
        }
        // Delete a submission
        private void DeleteSubmission(object sender, EventArgs e)
        {
            try
            {
               //check if user wants to delete
                if (DeleteSubmissionPicker.SelectedItem == null)
                {
                    DisplayAlert("Error", "Please select a submission to delete.", "OK"); // Notify if no submission is selected
                    return;
                }


                var selectedSubmission = DeleteSubmissionPicker.SelectedItem as Submission;// Get selected submission
                if (selectedSubmission == null) // Check if a submission is selected
                {
                    DisplayAlert("Error", "Please select a submission to delete.", "OK"); // Notify if no submission is selected
                    return;
                }

                using (var context = new dbContextCourses()) // Open database context
                {

                    var submission = context.Submissions.FirstOrDefault(s => s.Id == selectedSubmission.Id);// Find submission by ID
                    if (submission != null) // Check if submission exists
                    {
                        context.Submissions.Remove(submission); // Remove submission from database
                        context.SaveChanges(); // Save changes
                        DisplayAlert("Success", "Submission deleted successfully!", "OK"); // Notify success

                        // Refresh
                        RefreshSubmissionData();
                        ClearSubmissionInputs();
                        LoadSubmissionsIntoPicker();
                    }
                }
            }
            catch (Exception ex) // Apstrādā kļūdas gadījumā
            {
                DisplayAlert("Error", $"Error deleting submission: {ex.Message}", "OK"); // Show error message
            }
        }

        // Clear input fields related to submission data
        private void ClearSubmissionInputs()
        {
            EditSubmissionPicker.SelectedItem = null;
            EditSubmissionAssignmentPicker.SelectedItem = null;
            EditSubmissionScoreEntry.Text = string.Empty;
            StudentPicker.SelectedItem = null;
            AssignmentPicker.SelectedItem = null;
            SubmissionScoreEntry.Text = string.Empty;

            DeleteSubmissionPicker.SelectedItem = null;
        }

        // Refresh the submission data display 
        private void RefreshSubmissionData()
        {
            try
            {
                using (var context = new dbContextCourses())
                {
                    var submissions = context.Submissions
                        .Include(s => s.Student)
                        .Include(s => s.Assignment)
                        .Select(s => $"Student: {s.Student.FullName}, Assignment: {s.Assignment.Description}, Score: {s.Score}, Submitted: {s.SubmissionTime.ToShortDateString()}")
                        .ToList();

                    DataItems.Clear();
                    foreach (var submission in submissions)
                    {
                        DataItems.Add(submission);
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Error refreshing submissions: {ex.Message}", "OK"); // Show error message
            }
        }
    }
}


