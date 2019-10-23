using System;
using System.ComponentModel.DataAnnotations;

namespace GoogleCalendarApi.Models
{
    public class EventModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Summary { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Event Start")]
        public string Start { get; set; }
        [Required]
        [Display(Name = "Event End")]
        public string End { get; set; }
    }
}