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
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("name")]
        public string Name { get; set; } = null!;
        
        // 🔗 связь многие-ко-многим
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}