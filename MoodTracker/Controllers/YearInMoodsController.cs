using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoodTracker.Data;
using MoodTracker.ViewModels;
using MoodTracker.Utilities;
using MoodTracker.Services;

namespace MoodTracker.Controllers
{
    public class YearInMoodsController : Controller
    {
        private readonly DailyMoodService _dailyMoodService;
        private readonly MoodService _moodService;
        private readonly EventService _eventService;


        public YearInMoodsController(MoodTrackerContext context)
        {
            _dailyMoodService = new DailyMoodService(context);
            _moodService = new MoodService(context);
            _eventService = new EventService(context);
        }

        // GET: DailyMoods
        public async Task<IActionResult> Index()
        {
            YearInMoodsViewModel vm = new YearInMoodsViewModel
            {
                Dates = Calendar.GetElapsedDatesInPastYear(DateTime.Now.Year, DateTime.Now.Month),

                DailyMoods = await _dailyMoodService
                .GetDateDictOfDailyMoodsInDateRange(Calendar.GetMoodCalendarStartDate(), Calendar.GetMoodCalendarEndDate()),

                Events = await _eventService
                .GetDateDictOfEventsInDateRange(Calendar.GetMoodCalendarStartDate(), Calendar.GetMoodCalendarEndDate()),

                Moods = await _moodService.LoadAllMoods()
            };

            return View(vm);
        }

    }
}
