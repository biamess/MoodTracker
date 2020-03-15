using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoodTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodTracker.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MoodTrackerContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MoodTrackerContext>>()))
            {
                // Look for any DailyMoods.
                if (context.Moods.Any())
                {
                    return;   // DB has been seeded
                }

                Mood[] moodSeedData = new Mood[] {
                    new Mood
                    {
                        Id = 1,
                        Name = "Excited",
                        Color = "#dd3333"
                    },
                    new Mood
                    {
                        Id = 2,
                        Name = "Sad",
                        Color = "#3333ff"
                    },
                    new Mood
                    {
                        Id = 3,
                        Name = "Content",
                        Color = "#33dd33"
                    },
                    new Mood
                    {
                        Id = 4,
                        Name = "Worried",
                        Color = "#0000ff"
                    }
                };

                foreach (Mood m in moodSeedData)
                {
                    context.Moods.Add(m);
                }

                //context.Moods.AddRange();
                context.SaveChanges();
            }
        }
    }
}
