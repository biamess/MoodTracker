using Microsoft.EntityFrameworkCore;
using MoodTracker.Data;
using MoodTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodTracker.Services
{
    public class EventService
    {
        private readonly MoodTrackerContext _context;
        public EventService(MoodTrackerContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<DateTime,Event>> GetDateDictOfEventsInDateRange(DateTime startDate, DateTime endDate)
        {
            List<Event> events = await LoadEventsInDateRange(startDate, endDate);
            return events.ToDictionary(e => e.Date);
        }

        public async Task<List<Event>> LoadEventsInDateRange(DateTime startDate, DateTime endDate)
        {
            List<Event> events = await _context.Events
                .Where(e => e.Date >= startDate && e.Date <= endDate)
                .AsNoTracking()
                .ToListAsync();

            return events;
        }
    }
}
