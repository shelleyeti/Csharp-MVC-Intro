using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesMVC.Models
{
    public class Student
    {
        [Display(Name = "Student Id")]
        public int? Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Slack Handle")]
        [MinLength(2)]
        public string SlackHandle { get; set; }

        [Required]
        [Display(Name = "Cohort Id")]
        public int CohortId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public List<Cohort> Cohorts { get; set; } = new List<Cohort>();
    }
}
