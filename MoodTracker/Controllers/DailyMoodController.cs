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
                return RedirectToAction(nameof(Create), new { date = DateTime.Today });
            }
        }

        // GET: DailyMoods/Create
        public async Task<IActionResult> Create(DateTime? date)
        {
            DateTime defaultDate = date ?? DateTime.Today;

            DailyMoodViewModel dailyMoodVM = await GetDailyMoodVMForCreate(defaultDate.Date);

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

            DailyMoodViewModel vm = await GetDailyMoodVM(id.GetValueOrDefault());

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

            DailyMoodViewModel vm = await GetDailyMoodVM(id);
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

        public async Task<DailyMoodViewModel> GetDailyMoodVM(int dailyMoodId)
        {
            var dailyMood = await _context.DailyMoods.FindAsync(dailyMoodId);
            if (dailyMood == null)
            {
                return null;
            }

            DailyMoodViewModel dailyMoodVM = new DailyMoodViewModel
            {

                Id = dailyMood.Id,
                Date = dailyMood.Date,
                MoodId = dailyMood.MoodId,
                Notes = dailyMood.Notes,
                MoodIntensity = dailyMood.MoodIntensity,
                MoodList = await getMoodSelectList(dailyMood.MoodId)
        };
            return dailyMoodVM;
        }

        public async Task<DailyMoodViewModel> GetDailyMoodVMForCreate(DateTime date)
        {
            DailyMoodViewModel dailyMoodVM = new DailyMoodViewModel
            {
                Date = date,
                MoodList = await getMoodSelectList()
            };

            return dailyMoodVM;
        }

        /// <summary>
        /// Get a <see cref="SelectList"/> of user-defined moods.
        /// </summary>
        /// <returns>A selectlist of the user-defined moods. Data value is the mood Id, data text is the mood name.</returns>
        private async Task<SelectList> getMoodSelectList()
        {
            Dictionary<int, string> moods = await getMoodsDict();
            return new SelectList(moods, "Key", "Value");
        }

        /// <summary>
        /// Get a <see cref="SelectList"/> of user-defined moods.
        /// </summary>
        /// <param name="id">Id of mood to select in the <see cref="SelectList"/>.</param>
        /// <returns>A selectlist of the user-defined moods. Data value is the mood Id, data text is the mood name.</returns>
        private async Task<SelectList> getMoodSelectList(int id)
        {
            Dictionary<int, string> moods = await getMoodsDict();

            if (!moods.ContainsKey(id))
            {
                return new SelectList(moods, "Key", "Value");
            }
            else
            {
                return new SelectList(moods, "Key", "Value", id);
            }
        }

        /// <summary>
        /// Get a <see cref="Dictionary"/> of the user-defined moods.
        /// </summary>
        /// <returns> <see cref="Dictionary"/> of user-defined moods, with mood Ids as keys and mood names as values.</returns>
        private async Task<Dictionary<int,string>> getMoodsDict()
        {
            Dictionary<int, string> moods = await _context.Moods.ToDictionaryAsync(k => k.Id, v => v.Name);
            return moods;
        }
    }
}
