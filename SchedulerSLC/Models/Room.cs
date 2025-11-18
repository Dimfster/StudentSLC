using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentSLC.Models
{
    [Table("Rooms")]
    public class Room
    {
        [Key]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Required]
        [Column("type")]
        public string Type { get; set; } = null!;

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
