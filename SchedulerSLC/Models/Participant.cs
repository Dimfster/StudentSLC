using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentSLC.Models
{
    [Table("Participants")]
    public class Participant
    {
        [Key] 
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid(); // uuid в PostgreSQL → Guid в C#

        [Required]
        [Column("type")]
        public string Type { get; set; } = null!;

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
