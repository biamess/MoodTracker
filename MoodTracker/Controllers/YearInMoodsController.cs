using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodTracker.Data;
using MoodTracker.Models;
using MoodTracker.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
                Dates = GetDatesInYear(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

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

        /// <summary>
        /// For a given year and month, return a <list type="<list<DateTime>>">list</list> containing all the dates of the past year.
        /// </summary>
        /// <param name="year">Year</param>
        /// <param name="month">Month (0-11)</param>
        /// <returns><list type="<list<DateTime>>">list</list> containing the dates of the past 12 months (not including future dates this month).</returns>
        public static List<List<DateTime>> GetDatesInYear(int year, int month, int day)
        {
            List<List<DateTime>> dates = new List<List<DateTime>>();
            int currMonth = month;
            int currYear = year;
            int lastDay;

            for (int i = 1; i <= 12; i++)
            {
                if (currMonth == 0)
                {
                    currMonth = 12;
                    currYear--;
                }

                lastDay = (currYear == year && currMonth == month) ? day : DateTime.DaysInMonth(currYear, currMonth);
                dates.Add(GetDatesInMonth(currYear, currMonth, lastDay));

                currMonth -= 1;
            }
            return dates;
        }

        /// <summary>
        /// Get a <list type="<list<DateTime>>"> of days in the given month and year, up to the given day.
        /// </summary>
        /// <param name="year">Year to get dates for.</param>
        /// <param name="month">Month to get dates for.</param>
        /// <param name="day">Ending day for the range of dates.</param>
        /// <returns><list type="<list<DateTime>>"> of days in the given month and year, up to the given day.</returns>
        public static List<DateTime> GetDatesInMonth(int year, int month, int day)
        {
            return Enumerable.Range(1, day)  // Days: 1, 2 ... day
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList();
        }
    }
}
