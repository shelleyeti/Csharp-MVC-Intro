using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using StudentExercises.Models.ViewModels;
using StudentExercisesMVC.Models;

namespace StudentExercisesMVC.Controllers
{
    public class CohortsController : Controller
    {
        private readonly IConfiguration _config;

        public CohortsController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: Cohorts
        public ActionResult Index()
        {
            var cohorts = new List<Cohort>();

            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, [Name]
                                        FROM Cohort";

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cohorts.Add(new Cohort()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                        });
                    }
                    reader.Close();
                }
            return View(cohorts);
            }
        }

        // GET: Cohorts/Details/5
        public ActionResult Details(int id)
        {
            Cohort cohort= null;
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, [Name]
                                        FROM Cohort
                                        WHERE Id = @id";

                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        cohort = new Cohort()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("[Name]")),
                        };
                    }
                }
            }
            return View(cohort);
        }

        // GET: Cohorts/Create
        public ActionResult Create()
        {
            var viewModel = new CohortCreateViewModel();
            var cohorts = GetAllCohorts();
            var selectItems = cohorts
                .Select(cohort => new SelectListItem
                {
                    Text = cohort.Name,
                    Value = cohort.Id.ToString()
                })
                .ToList();

            selectItems.Insert(0, new SelectListItem
            {
                Text = "Choose cohort...",
                Value = "0"
            });
            //viewModel.Cohorts = selectItems;
            return View(viewModel);
        }

        // POST: Cohorts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cohort cohort)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Cohort
                                            ([Name])
                                            VALUES 
                                            (@name)";

                        cmd.Parameters.AddWithValue("@name", cohort.Name);

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //// GET: Instructors/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    var viewModel = new InstructorEditViewModel();
        //    var cohorts = GetAllCohorts();
        //    var selectItems = cohorts
        //        .Select(cohort => new SelectListItem
        //        {
        //            Text = cohort.Name,
        //            Value = cohort.CohortId.ToString()
        //        })
        //        .ToList();

        //    selectItems.Insert(0, new SelectListItem
        //    {
        //        Text = "Choose cohort...",
        //        Value = "0"
        //    });
        //    viewModel.Cohorts = selectItems;

        //    Instructor instructor = null;
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"SELECT Id, FirstName, LastName, SlackHandle, CohortId
        //                                FROM Instructor
        //                                WHERE Id = @id";

        //            cmd.Parameters.Add(new SqlParameter("@id", id));
        //            SqlDataReader reader = cmd.ExecuteReader();

        //            if (reader.Read())
        //            {
        //                instructor = new Instructor()
        //                {
        //                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
        //                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
        //                    SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
        //                    CohortId = reader.GetInt32(reader.GetOrdinal("CohortId"))
        //                };
        //            }
        //        }
        //    }

        //    foreach (var cohortItem in viewModel.Cohorts)
        //    {
        //        if (Convert.ToInt32(cohortItem.Value) == instructor.CohortId)
        //        {
        //            cohortItem.Selected = true;
        //        }
        //    }

        //    viewModel.Instructor = instructor;
        //    return View(viewModel);
        //}

        //// POST: Instructors/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, Instructor instructor)
        //{
        //    try
        //    {
        //        using (SqlConnection conn = Connection)
        //        {
        //            conn.Open();
        //            using (SqlCommand cmd = conn.CreateCommand())
        //            {
        //                cmd.CommandText = @"UPDATE Instructor
        //                                    SET 
        //                                    FirstName = @firstName, 
        //                                    LastName = @lastName, 
        //                                    SlackHandle = @slackHandle, 
        //                                    CohortId = @cohortId
        //                                    WHERE Id = @id";

        //                cmd.Parameters.AddWithValue("@firstName", instructor.FirstName);
        //                cmd.Parameters.AddWithValue("@lastName", instructor.LastName);
        //                cmd.Parameters.AddWithValue("@slackHandle", instructor.SlackHandle);
        //                cmd.Parameters.AddWithValue("@cohortId", instructor.CohortId);
        //                cmd.Parameters.AddWithValue("@id", id);

        //                cmd.ExecuteNonQuery();
        //            }
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch (Exception err)
        //    {
        //        return View();
        //    }
        //}

        //// GET: Instructors/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Instructors/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

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
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
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