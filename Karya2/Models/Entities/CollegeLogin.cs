using System.ComponentModel.DataAnnotations;

namespace Kriya2.Models.Entities
{
    public class CollegeLogin
    {
        [Key] // UserName as primary key
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = string.Empty;

        // Optional: Link login to College entity
        public int CollegeId { get; set; }
    }
}
