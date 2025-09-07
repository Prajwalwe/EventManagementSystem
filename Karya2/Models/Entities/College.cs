using System.ComponentModel.DataAnnotations;

namespace Kriya2.Models.Entities
{
    public class College
    {
        [Key]
        public int CollegeId { get; set; }    // PK

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

    }
}
