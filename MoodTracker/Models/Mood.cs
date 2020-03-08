using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodTracker.Models
{
    public class Mood
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PercentIntensity { get; set; }
    }
}
