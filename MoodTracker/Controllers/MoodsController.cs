using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodTracker.Data;
using MoodTracker.Models;
using MoodTracker.Services;


namespace MoodTracker.Controllers
{
    public class MoodsController : Controller
    {
        private readonly MoodService _moodService;

        public MoodsController(MoodTrackerContext context)
        {
            _moodService = new MoodService(context);
        }

        // GET: Moods
        public async Task<IActionResult> Index()
        {
            return View(await _moodService.LoadAllMoods());
        }

        // GET: Moods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mood = await _moodService.GetUntrackedMoodWithId(id.GetValueOrDefault());
            if (mood == null)
            {
                return NotFound();
            }

            return View(mood);
        }

        // GET: Moods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Moods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Color")] Mood mood)
        {
            if (ModelState.IsValid)
            {
                _moodService.AddMood(mood);
                await _moodService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mood);
        }

        // GET: Moods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mood = await _moodService.GetTrackedMoodWithId(id.GetValueOrDefault());
            if (mood == null)
            {
                return NotFound();
            }
            return View(mood);
        }

        // POST: Moods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Color")] Mood mood)
        {
            if (id != mood.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _moodService.UpdateMood(mood);
                    await _moodService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_moodService.MoodExists(mood.Id))
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
            return View(mood);
        }

        // GET: Moods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mood = await _moodService.GetUntrackedMoodWithId(id.GetValueOrDefault());
            if (mood == null)
            {
                return NotFound();
            }

            return View(mood);
        }

        // POST: Moods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mood = await _moodService.GetTrackedMoodWithId(id);
            _moodService.RemoveMood(mood);

            await _moodService.DeleteDailyMoodsWithMood(mood.Id);

            await _moodService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
