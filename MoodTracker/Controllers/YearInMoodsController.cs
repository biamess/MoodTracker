using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodTracker.Data;
using MoodTracker.Models;
using MoodTracker.ViewModels;
using MoodTracker.Utilities;

namespace MoodTracker.Controllers
{
    public class YearInMoodsController : Controller
    {
        private readonly MoodTrackerContext _context;

        public YearInMoodsController(MoodTrackerContext context)
        {
            _context = context;
        }

        // GET: DailyMoods
        public async Task<IActionResult> Index()
        {
            YearInMoodsViewModel vm = new YearInMoodsViewModel
            {
                Dates = Calendar.GetElapsedDatesInPastYear(DateTime.Now.Year, DateTime.Now.Month),

                DailyMoods = await _context.DailyMoods
                .Include(d => d.Mood)
                .AsNoTracking()
                .ToDictionaryAsync(k => k.Date, v => v),

                Events = await _context.Events
                .AsNoTracking()
                .ToDictionaryAsync(k => k.Date, v => v),

                Moods = await _context.Moods.ToListAsync<Mood>()
            };

            return View(vm);
        }

    }
}
