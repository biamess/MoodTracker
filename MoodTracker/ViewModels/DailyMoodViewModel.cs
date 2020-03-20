using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoodTracker.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MoodTracker.ViewModels
{
    public class DailyMoodViewModel {   

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [BindProperty]
        public int MoodId { get; set; }

        [BindProperty]
        public string Notes { get; set; }
        public SelectList MoodList { get; set; }
    }
}
