using MoodTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodTracker.ViewModels
{
    public class DailyMoodIndexViewModel
    {
        public List<List<DateTime>> Dates { get; set; }

        public Dictionary<DateTime, DailyMood> DailyMoods { get; set; }
    }
}
