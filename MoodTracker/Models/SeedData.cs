using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoodTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodTracker.Models
{
    public class SeedData
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

                context.Moods.AddRange(
                    new Mood
                    {
                        Id = 1,
                        Name = "excited",
                        Color = "#dd3333"
                    },
                    new Mood
                    {
                        Id = 2,
                        Name = "sad",
                        Color = "#3333ff"
                    },
                    new Mood
                    {
                        Id = 3,
                        Name = "content",
                        Color = "#33dd33"
                    },
                    new Mood
                    {
                        Id = 4,
                        Name = "worried",
                        Color = "#0000ff"
                    }
                    );

                //context.Moods.AddRange();
                context.SaveChanges();
            }
        }
    }
}
