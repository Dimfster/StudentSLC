using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentSLC.Models
{
    [Table("Groups")] 
    public class Group

    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid(); // uuid в PostgreSQL → Guid в C#

        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;
        
        // связь М:М с Users
        public ICollection<User> Users { get; set; } = new List<User>();

        // связь 1:1 с Participant
        public Participant Participant { get; set; } = null!;
    }
}