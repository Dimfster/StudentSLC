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
        [Column("place")]
        public string Place { get; set; } = null!;

        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
        public ICollection<Participant> KeyHolders { get; set; } = new List<Participant>();
    }
}
