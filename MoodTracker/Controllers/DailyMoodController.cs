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

                Dates = GetDatesInMonth(DateTime.Now.Year, DateTime.Now.Month),

                DailyMoods = await _context.DailyMoods
                .Include(d => d.Mood)
                .AsNoTracking()
                .ToDictionaryAsync(k => k.Date, v => v)
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyMood == null)
            {
                return NotFound();
            }

            return View(dailyMood);
        }

        // GET: DailyMoods/Create
        public async Task<IActionResult> Create()
        {
            DailyMoodViewModel dailyMoodVM = new DailyMoodViewModel
            {
                Date = DateTime.Today,
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

        public static List<DateTime> GetDatesInMonth(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList(); // Load dates into a list
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
