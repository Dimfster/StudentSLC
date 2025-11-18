using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentSLC.Models
{
    [Table("Events")]
    public class Event
    {
        [Key]
        [Column("name")]
        public string Name { get; set; } = null!;

        [Required]
        [Column("start_time")]
        public DateTime StartTime { get; set; } // timestamp → DateTime

        [Required]
        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Required]
        [Column("room")]
        public string RoomName { get; set; } = null!;  // FK → Rooms(name)

        // навигационное свойство 1:M
        public Room Room { get; set; } = null!;

        // связь М:М с Participants
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();

        // связь М:М с KeyHolders
        public ICollection<Participant> KeyHolders { get; set; } = new List<Participant>();
    }
}
