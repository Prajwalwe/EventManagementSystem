using System.ComponentModel.DataAnnotations;

namespace Kriya2.Models.Entities
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }   // PK

        [Required]
        public int AttendanceId { get; set; }   // FK to Attendance

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }   // Example: 1–5 rating

        public string Summary { get; set; } = string.Empty;
    }
}
