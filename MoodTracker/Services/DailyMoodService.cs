using Microsoft.EntityFrameworkCore;
using MoodTracker.Data;
using MoodTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodTracker.Services
{
    public class DailyMoodService
    {
        private readonly MoodTrackerContext _context;
        public DailyMoodService(MoodTrackerContext context)
        {
            _context = context;
        }

        public async Task<DailyMood> GetUntrackedDailyMoodWithDate(DateTime date)
        {
            return await _context.DailyMoods.Where(d => d.Date == date).FirstOrDefaultAsync();
        }

        public async Task<DailyMood> GetUntrackedDailyMoodWithId(int id)
        {
            DailyMood dailyMood = await _context.DailyMoods
                .AsNoTracking()
                .Include(d => d.Mood)
                .FirstOrDefaultAsync(m => m.Id == id);
            return dailyMood;
        }

        public async Task<DailyMood> GetTrackedDailyMoodWithId(int id)
        {
            DailyMood dailyMood = await _context.DailyMoods
                .Include(d => d.Mood)
                .FirstOrDefaultAsync(m => m.Id == id);
            return dailyMood;
        }

        public bool DailyMoodExists(int id)
        {
            return _context.DailyMoods.Any(d => d.Id == id);
        }

        public void AddDailyMood(DailyMood dailyMood)
        {
            _context.DailyMoods.Add(dailyMood);
        }

        public void UpdateDailyMood(DailyMood dailyMood)
        {
            _context.DailyMoods.Update(dailyMood);
        }

        public void RemoveDailyMood(DailyMood dailyMood)
        {
            _context.DailyMoods.Remove(dailyMood);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<DateTime, DailyMood>> GetDateDictOfDailyMoodsInDateRange(DateTime startDate, DateTime endDate)
        {
            List<DailyMood> dailyMoods = await GetDailyMoodsInDateRange(startDate, endDate);
            return dailyMoods.ToDictionary(d => d.Date);
        }

        public async Task<List<DailyMood>> GetDailyMoodsInDateRange(DateTime startDate, DateTime endDate)
        {
            List<DailyMood> dailyMoods = await _context.DailyMoods
                .Include(d => d.Mood)
                .Where(d => d.Date >= startDate && d.Date <= endDate)
                .AsNoTracking()
                .ToListAsync();

            return dailyMoods;
        }
    }
}
