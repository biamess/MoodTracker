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

        public async Task<Event> GetUntrackedEventWithId(int id)
        {
            Event eventWithId = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
            return eventWithId;
        }

        public async Task<Event> GetTrackedEventWithId(int id)
        {
            Event eventWithId = await _context.Events
              .FirstOrDefaultAsync(e => e.Id == id);
            return eventWithId;
        }

        public bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        public void AddEvent(Event eventToAdd)
        {
            _context.Events.Add(eventToAdd);
        }

        public void UpdateEvent(Event eventToUpdate)
        {
            _context.Events.Update(eventToUpdate);
        }

        public void RemoveEvent(Event eventToRemove)
        {
            _context.Events.Remove(eventToRemove);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<DateTime,Event>> GetDateDictOfEventsInDateRange(DateTime startDate, DateTime endDate)
        {
            List<Event> events = await GetEventsInDateRange(startDate, endDate);
            return events.ToDictionary(e => e.Date);
        }

        public async Task<List<Event>> GetEventsInDateRange(DateTime startDate, DateTime endDate)
        {
            List<Event> events = await _context.Events
                .Where(e => e.Date >= startDate && e.Date <= endDate)
                .AsNoTracking()
                .ToListAsync();

            return events;
        }

        public async Task<List<Event>> GetAllEvents()
        {
            List<Event> events = await _context.Events
                .AsNoTracking()
                .ToListAsync();

            return events;
        }
    }
}
