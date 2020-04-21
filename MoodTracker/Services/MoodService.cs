using MoodTracker.Data;
using MoodTracker.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace MoodTracker.Services
{
    public class MoodService
    {
        private readonly MoodTrackerContext _context;
        public MoodService(MoodTrackerContext context)
        {
            _context = context;
        }

        public async Task<Mood> GetUntrackedMoodWithId(int id)
        {
            Mood mood = await _context.Moods
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            return mood;
        }

        public async Task<Mood> GetTrackedMoodWithId(int id)
        {
            Mood mood = await _context.Moods
                .FirstOrDefaultAsync(m => m.Id == id);
            return mood;
        }

        public bool MoodExists(int id)
        {
            return _context.Moods.Any(d => d.Id == id);
        }

        public void AddMood(Mood mood)
        {
            _context.Moods.Add(mood);
        }

        public void UpdateMood(Mood mood)
        {
            _context.Moods.Update(mood);
        }

        public void RemoveMood(Mood mood)
        {
            _context.Moods.Remove(mood);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task DeleteDailyMoodsWithMood(int moodId)
        {
            List<DailyMood> dailyMoods = await _context.DailyMoods.Where(d => d.MoodId == moodId).ToListAsync();
            foreach (DailyMood dm in dailyMoods)
            {
                _context.DailyMoods.Remove(dm);
            }
        }

        /// <summary>
        /// Get a <see cref="Dictionary"/> of the user-defined moods.
        /// </summary>
        /// <returns> <see cref="Dictionary"/> of user-defined moods, with mood Ids as keys and mood names as values.</returns>
        public async Task<Dictionary<int, string>> GetMoodNameDict()
        {
            return await _context.Moods.ToDictionaryAsync(k => k.Id, v => v.Name);
        }

        public async Task<List<Mood>> GetAllMoods()
        {
            List<Mood> moods = await _context.Moods
                .AsNoTracking()
                .ToListAsync();

            return moods;
        }
    }
}
