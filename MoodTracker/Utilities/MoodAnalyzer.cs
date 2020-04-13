using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoodTracker.Data;
using MoodTracker.Services;

namespace MoodTracker.Utilities
{
    public class MoodAnalyzer
    {
        private DailyMoodService _dailyMoodService;

        public MoodAnalyzer(MoodTrackerContext context)
        {
            _dailyMoodService = new DailyMoodService(context);
        }
    }
}
