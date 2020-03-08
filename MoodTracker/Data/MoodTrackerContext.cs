using Microsoft.EntityFrameworkCore;
using MoodTracker.Models;

namespace MoodTracker.Data
{
    public class MoodTrackerContext : DbContext
    {
        public MoodTrackerContext(DbContextOptions<MoodTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<DailyMood> DailyMood { get; set; }
    }
}