using System.ComponentModel.DataAnnotations;

namespace StudentExercisesMVC.Models
{
    public class Cohort
    {
        [Display(Name = "Cohort Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Cohort Name")]
        public string Name { get; set; }
    }
}