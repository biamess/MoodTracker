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
        public async Task<IActionResult> LogMoodForToday()
        {
            DailyMood moodForToday = await _context.DailyMoods.Where(d => d.Date == DateTime.Today).FirstOrDefaultAsync();

            if (moodForToday != null)
            {
                return RedirectToAction(nameof(Edit), new { id = moodForToday.Id });
            }
            else
            {
                return RedirectToAction(nameof(Create), new { date = DateTime.Today } );
            }
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
        public async Task<IActionResult> Create([Bind("Id,Date,MoodId,MoodIntensity,Notes")] DailyMoodViewModel vm)
        {
           DailyMood dailyMood = new DailyMood();

            if (ModelState.IsValid)
            {
                dailyMood.Date = vm.Date;
                dailyMood.MoodId = vm.MoodId;
                dailyMood.MoodIntensity = vm.MoodIntensity;
                dailyMood.Notes = vm.Notes;
                dailyMood.InputTimestamp = DateTime.Now;

                _context.Add(dailyMood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "YearInMoods");
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,MoodId,MoodIntensity,Notes")] DailyMood dailyMood)
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
                return RedirectToAction(nameof(Index), "YearInMoods");
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
            return RedirectToAction(nameof(Index), "YearInMoods");
        }

        private bool DailyMoodExists(int id)
        {
            return _context.DailyMoods.Any(e => e.Id == id);
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
