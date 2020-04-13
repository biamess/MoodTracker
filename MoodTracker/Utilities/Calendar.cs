using System;
using System.Collections.Generic;
using System.Linq;

namespace MoodTracker.Utilities
{
    public static class Calendar
    {
        public static DateTime GetMoodCalendarStartDate()
        {
            const int startingDay = 1;
            int startingMonth = DateTime.Today.Month == 12 ? 1 : DateTime.Today.Month + 1;
            int startingYear = startingMonth == 1 ? DateTime.Today.Year : DateTime.Today.Year - 1;

            return new DateTime(startingYear, startingMonth, startingDay);
        }

        public static DateTime GetMoodCalendarEndDate()
        {
            return DateTime.Today;
        }

        /// <summary>
        /// For a given year and month, return a <list type="<list<DateTime>>">list</list> containing all the dates, 
        /// up to today, in the past 12 months.
        /// </summary>
        /// <param name="year">Year</param>
        /// <param name="month">Month</param>
        /// <returns><list type="<list<DateTime>>">list</list> containing the dates of the past 12 
        /// months up to today. </returns>
        public static List<List<DateTime>> GetElapsedDatesInPastYear(int year, int month)
        {
            List<List<DateTime>> dates = GetDatesInPastYear(year, month);
            RemoveFutureDates(dates);
            return dates;
        }

        /// <summary>
        /// For a given year and month, return a <list type="<list<DateTime>>">list</list> containing all the 
        /// dates of the past 12 months (including the given month).
        /// </summary>
        /// <param name="year">Year</param>
        /// <param name="month">Month</param>
        /// <returns><list type="<list<DateTime>>">list</list> containing the dates of the past 12 months. </returns>
        public static List<List<DateTime>> GetDatesInPastYear(int year, int month)
        {
            List<List<DateTime>> dates = new List<List<DateTime>>();
            int currMonth = month;
            int currYear = year;

            for (int i = 1; i <= 12; i++)
            {
                if (currMonth == 0)
                {
                    currMonth = 12;
                    currYear--;
                }

                dates.Add(GetDatesInMonth(currYear, currMonth));

                currMonth -= 1;
            }
            return dates;
        }

        /// <summary>
        /// Get a <list type="<list<DateTime>>"> of days in the given month and year.
        /// </summary>
        /// <param name="year">Year to get dates for.</param>
        /// <param name="month">Month to get dates for.</param>
        /// <returns><list type="<list<DateTime>>"> of days in the given month of the given year. </returns>
        public static List<DateTime> GetDatesInMonth(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month)) 
                             .Select(day => new DateTime(year, month, day)) // Map each day to a date
                             .ToList();
        }

        public static void RemoveFutureDates(List<List<DateTime>> dates)
        {
            for (int i=0; i<dates.Count; i++)
            {
                dates[i].RemoveAll(IsFutureDate);
            }
        }

        private static bool IsFutureDate (DateTime date)
        {
            return date > DateTime.Today;
        }
    }
}
