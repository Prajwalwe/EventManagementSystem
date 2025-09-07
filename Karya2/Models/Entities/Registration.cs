using System.ComponentModel.DataAnnotations;

namespace Kriya2.Models.Entities
{
    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }   // PK

        [Required]
        public int EventId { get; set; }          // FK reference to Event

        [Required]
        public int StudentId { get; set; }        // FK reference to Student
    }
}
