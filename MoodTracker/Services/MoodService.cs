using MoodTracker.Data;
using MoodTracker.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MoodTracker.Services
{
    public class MoodService
    {
        private readonly MoodTrackerContext _context;
        public MoodService(MoodTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a <see cref="Dictionary"/> of the user-defined moods.
        /// </summary>
        /// <returns> <see cref="Dictionary"/> of user-defined moods, with mood Ids as keys and mood names as values.</returns>
        public async Task<Dictionary<int, string>> GetMoodNameDict()
        {
            return await _context.Moods.ToDictionaryAsync(k => k.Id, v => v.Name);
        }

        public async Task<List<Mood>> LoadAllMoods()
        {
            List<Mood> moods = await _context.Moods
                .AsNoTracking()
                .ToListAsync();

            return moods;
        }
    }
}
