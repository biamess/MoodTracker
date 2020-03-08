using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoodTracker.Data;
using MoodTracker.Models;

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
            return View(await _context.DailyMood.ToListAsync());
        }

        // GET: DailyMoods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyMood = await _context.DailyMood
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dailyMood == null)
            {
                return NotFound();
            }

            return View(dailyMood);
        }

        // GET: DailyMoods/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DailyMoods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Notes")] DailyMood dailyMood)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyMood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dailyMood);
        }

        // GET: DailyMoods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyMood = await _context.DailyMood.FindAsync(id);
            if (dailyMood == null)
            {
                return NotFound();
            }
            return View(dailyMood);
        }

        // POST: DailyMoods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Notes")] DailyMood dailyMood)
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
            return View(dailyMood);
        }

        // GET: DailyMoods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailyMood = await _context.DailyMood
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
            var dailyMood = await _context.DailyMood.FindAsync(id);
            _context.DailyMood.Remove(dailyMood);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyMoodExists(int id)
        {
            return _context.DailyMood.Any(e => e.Id == id);
        }
    }
}
