using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using MVC.HabitTracker.JsPeanut.Models;
using System.Globalization;

namespace MVC.HabitTracker.JsPeanut.Pages
{
    public class DeleteModel : PageModel
    {
        public IConfiguration _configuration { get; set; }

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult OnGet(int id)
        {
            HabitLog = GetById(id);
            return Page();
        }

        [BindProperty]
        public HabitLog HabitLog { get; set; }

        private HabitLog GetById(int id)
        {
            var habitLogRecord = new HabitLog();

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM habit_logs WHERE Id = {id}";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                while (reader.Read())
                {
                    habitLogRecord.Id = reader.GetInt32(0);
                    habitLogRecord.HabitTypeName = reader.GetString(1);
                    habitLogRecord.Date = DateTime.Parse(reader.GetString(2), CultureInfo.InstalledUICulture);
                    habitLogRecord.Quantity = reader.GetInt32(3);
                }

                return habitLogRecord;
            }
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"DELETE FROM habit_logs WHERE Id = {id}";

                tableCmd.ExecuteNonQuery();
            }

            return RedirectToPage("./Index");
        }
    }
}