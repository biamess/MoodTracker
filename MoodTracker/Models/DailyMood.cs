using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MoodTracker.Models
{
    public class DailyMood
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int MoodId { get; set; }

        [ForeignKey("MoodId")]
        public Mood Mood { get; set; }

        public string Notes { get; set; }
    }
}
