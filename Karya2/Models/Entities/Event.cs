using System;
using System.ComponentModel.DataAnnotations;

namespace Kriya2.Models.Entities
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }   // PK

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int CollegeId { get; set; }  // FK reference to College

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartDateTime { get; set; }  // Start date & time
        [Required]
        public int EventTypeId { get; set; }   // FK reference to EventType

        [Required]
        public int EventStatus { get; set; }   // 0 = Inactive, 1 = Active
    }
}
