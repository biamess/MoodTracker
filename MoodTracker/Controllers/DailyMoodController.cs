using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoodTracker.Data;
using MoodTracker.Models;
using MoodTracker.ViewModels;

namespace MoodTracker.Controllers
{
    public class DailyMoodController : Controller
    {
        private readonly MoodTrackerContext _context;

        public DailyMoodController(MoodTrackerContext context)
        {
            _context = context;
        }

        // GET: DailyMoods
        public async Task<IActionResult> Index()
        {
            DailyMoodIndexViewModel vm = new DailyMoodIndexViewModel
            {
                Dates = GetDatesInYear(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),

                DailyMoods = await _context.DailyMoods
                .Include(d => d.Mood)
                .AsNoTracking()
                .ToDictionaryAsync(k => k.Date, v => v),

                Moods = await _context.Moods.ToListAsync<Mood>()
        };

            return View(vm);
        }

        // GET: DailyMoods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyMood = await _context.DailyMoods
                .AsNoTracking()
                .Include(d => d.Mood)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (dailyMood == null)
            {
                return NotFound();
            }

            return View(dailyMood);
        }

        // GET: DailyMoods/Create
        public async Task<IActionResult> Create(DateTime? date)
        {
            DateTime defaultDate = date ?? DateTime.Today;

            DailyMoodViewModel dailyMoodVM = new DailyMoodViewModel
            {
                Date = defaultDate.Date,
                MoodList = new SelectList(await _context.Moods.ToDictionaryAsync(k => k.Id, v => v.Name), "Key", "Value")
            };

            return View(dailyMoodVM);
        }

        // POST: DailyMoods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,MoodId,Notes")] DailyMoodViewModel vm)
        {
           DailyMood dailyMood = new DailyMood();

            if (ModelState.IsValid)
            {
                dailyMood.Date = vm.Date;
                dailyMood.MoodId = vm.MoodId;
                dailyMood.Notes = vm.Notes;
                dailyMood.InputTimestamp = DateTime.Now;

                _context.Add(dailyMood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.MoodList = new SelectList(await _context.Moods.ToDictionaryAsync(k => k.Id, v => v.Name), "Key", "Value");

            return View(vm);
        }

        // GET: DailyMoods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DailyMoodViewModel vm = await GetDailyMoodViewModel(id.GetValueOrDefault());

            return View(vm);
        }

        // POST: DailyMoods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,MoodId,Notes")] DailyMood dailyMood)
        {
            if (id != dailyMood.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyMood);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyMoodExists(dailyMood.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            DailyMoodViewModel vm = await GetDailyMoodViewModel(id);
            return View(vm);
        }

        // GET: DailyMoods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyMood = await _context.DailyMoods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyMood == null)
            {
                return NotFound();
            }

            return View(dailyMood);
        }

        // POST: DailyMoods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailyMood = await _context.DailyMoods.FindAsync(id);
            _context.DailyMoods.Remove(dailyMood);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyMoodExists(int id)
        {
            return _context.DailyMoods.Any(e => e.Id == id);
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
            int currMonth = month + 1;
            int currYear = year - 1;
            int lastDay;

            for (int i=1; i<=12; i++)
            {
                if (currMonth == 13)
                {
                    currMonth = 1;
                    currYear++;
                }

                lastDay = (currYear == year && currMonth == month) ? day : DateTime.DaysInMonth(currYear, currMonth);
                dates.Add(GetDatesInMonth(currYear, currMonth, lastDay));

                currMonth += 1;
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

        public async Task<DailyMoodViewModel> GetDailyMoodViewModel(int dailyMoodId)
        {
            var dailyMood = await _context.DailyMoods.FindAsync(dailyMoodId);
            if (dailyMood == null)
            {
                return null;
            }

            Dictionary<int, string> moods = await _context.Moods.ToDictionaryAsync(k => k.Id, v => v.Name);

            DailyMoodViewModel dailyMoodVM = new DailyMoodViewModel
            {

                Id = dailyMood.Id,
                Date = dailyMood.Date,
                MoodId = dailyMood.MoodId,
                Notes = dailyMood.Notes,
                MoodList = new SelectList(moods, "Key", "Value", moods[dailyMood.MoodId])
            };
            return dailyMoodVM;
        }
    }
}
