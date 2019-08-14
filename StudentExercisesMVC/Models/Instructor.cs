using System.ComponentModel.DataAnnotations;

namespace StudentExercisesMVC.Models
{
    public class Instructor
    {
        [Display(Name = "Instructor Id")]
        public int? Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Slack Handle")]
        public string SlackHandle { get; set; }

        [Required]
        [Display(Name = "Cohort Id")]
        public int CohortId { get; set; }

        [Display(Name = "Specialty")]
        public string Specialty { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
