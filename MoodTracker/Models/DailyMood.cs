using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoodTracker.Models
{
    public class DailyMood
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public Mood Mood { get; set; }

        public string Notes { get; set; }
    }
}
