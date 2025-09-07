using System.ComponentModel.DataAnnotations;

namespace Kriya2.Models.Entities
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }    // PK

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public int CollegeId { get; set; }    // FK reference only

        [Required]
        public string USN { get; set; } = string.Empty;
    }
}
