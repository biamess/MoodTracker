using Microsoft.AspNetCore.Mvc.Rendering;
using MoodTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodTracker.ViewModels
{
    public class DailyMoodViewModel
    {
        public DailyMood DailyMood { get; set; }

        public SelectList MoodList { get; set; }
    }
}
