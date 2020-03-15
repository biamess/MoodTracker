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

        public DbSet<DailyMood> DailyMoods { get; set; }

        public DbSet<Mood> Moods { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DailyMood>().ToTable("DailyMood");
            modelBuilder.Entity<Mood>().ToTable("Mood");
        }
    }
}