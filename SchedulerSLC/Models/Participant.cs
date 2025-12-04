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

        public ICollection<Event> EventsAsParticipant { get; set; } = new List<Event>();

        public ICollection<Event> EventsAsKeyHolder { get; set; } = new List<Event>();

        // связь 1:1 с User
        public User? User { get; set; } 

        // связь 1:1 с Group
        public Group? Group { get; set; } 
    }
}
