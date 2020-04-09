using MoodTracker.Models;
using System;
using System.Collections.Generic;

namespace MoodTracker.ViewModels
{
    public class DailyMoodIndexViewModel
    {
        public List<List<DateTime>> Dates { get; set; }

        public Dictionary<DateTime, DailyMood> DailyMoods { get; set; }

        public Dictionary<DateTime, Event> Events { get; set; }

        public List<Mood> Moods { get; set; }
    }
}
