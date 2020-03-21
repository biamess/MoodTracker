using System.ComponentModel;


namespace MoodTracker.Models
{
    public class Mood
    {
        public int Id { get; set; }

        [DisplayName("Mood")]
        public string Name { get; set; }

        public string Color { get; set; }
    }
}
