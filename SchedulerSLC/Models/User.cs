using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentSLC.Models
{
    [Table("Users")] 
    public class User
    {
        [Key] 
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();  // uuid в PostgreSQL → Guid в C#

        [Required] 
        [Column("first_name")]
        public string FirstName { get; set; } = null!; 

        [Required]
        [Column("last_name")]
        public string LastName { get; set; } = null!;

        [Column("patronymic")]
        public string? Patronymic { get; set; }

        [Required]
        [Column("role")]
        public string Role { get; set; } = null!;

        // связь М:М с Roles
        public ICollection<Role> Roles { get; set; } = new List<Role>();

        // связь М:М с Groups
        public ICollection<Group> Groups { get; set; } = new List<Group>();

        // связь 1:1 с Participant
        public Participant Participant { get; set; } = null!;
    }
}
