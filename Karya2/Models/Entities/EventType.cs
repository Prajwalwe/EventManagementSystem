using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kriya2.Models.Entities
{
    public class EventType
    {
        [Key]
        public int EventTypeId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
