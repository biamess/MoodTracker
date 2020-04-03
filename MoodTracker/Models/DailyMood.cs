using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MoodTracker.Models
{
    public class DailyMood
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public int MoodId { get; set; }

        [ForeignKey("MoodId")]
        public Mood Mood { get; set; }

        public string Notes { get; set; }

        [DisplayName("Mood Intensity")]
        public double MoodIntensity { get; set; }

        public DateTime InputTimestamp { get; set; }
    }
}
