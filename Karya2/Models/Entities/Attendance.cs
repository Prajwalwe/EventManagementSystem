using System.ComponentModel.DataAnnotations;

namespace Kriya2.Models.Entities
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }       // PK

        [Required]
        public int RegistrationId { get; set; }     // FK reference to Registration

        [Required]
        public bool IsPresent { get; set; }         // true = present, false = absent
    }
}
