using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentSLC.Models
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [Column("name")]
        public string Name { get; set; } = null!;

        // связь M:M с Users
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}