using Microsoft.AspNetCore.Mvc.Rendering;
using StudentExercisesMVC.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace StudentExercises.Models.ViewModels
{
    public class StudentCreateViewModel
    {
        public List<SelectListItem> Cohorts { get; set; }
        public Student Student { get; set; }

        private readonly string _connectionString;

        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public StudentCreateViewModel() { }

        public StudentCreateViewModel(string connectionString)
        {
            _connectionString = connectionString;

            Cohorts = GetAllCohorts()
                .Select(cohort => new SelectListItem
                {
                    Text = cohort.Name,
                    Value = cohort.CohortId.ToString()
                })
                .ToList();
                Cohorts.Insert(0, new SelectListItem
                {
                    Text = "Choose cohort...",
                    Value = "0"
                });
        }

        private List<Cohort> GetAllCohorts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Cohort";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Cohort> cohorts = new List<Cohort>();
                    while (reader.Read())
                    {
                        cohorts.Add(new Cohort
                        {
                            CohortId = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        });
                    }

                    reader.Close();

                    return cohorts;
                }
            }
        }
    }
}