using Microsoft.EntityFrameworkCore;

using StudentSLC.Models;

namespace StudentSLC.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Participant> Participants { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Role> Roles {get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Users_Roles M:M
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("Users_Roles"));

            // Users_Groups M:M
            modelBuilder.Entity<User>()
                .HasMany(u => u.Groups)
                .WithMany(g => g.Users)
                .UsingEntity(j => j.ToTable("Users_Groups"));
            
            // Users_Roles M:M
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("Users_Roles"));

            // 👥 Events_Participants M:M
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Participants)
                .WithMany(p => p.EventsAsParticipant)
                .UsingEntity(j => j.ToTable("Events_Participants"));

            // Events_KeyHolders M:M
            modelBuilder.Entity<Event>()
                .HasMany(e => e.KeyHolders)
                .WithMany(p => p.EventsAsKeyHolder)
                .UsingEntity(j => j.ToTable("Events_KeyHolders"));

            // User - Participant 1:1
            modelBuilder.Entity<User>()
                .HasOne(u => u.Participant)
                .WithOne(p => p.User)
                .HasForeignKey<User>(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Group - Participant 1:1
            modelBuilder.Entity<Group>()
                .HasOne(u => u.Participant)
                .WithOne(p => p.Group)
                .HasForeignKey<Group>(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Room - Events 1:M
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Room)
                .WithMany(r => r.Events)
                .HasForeignKey(e => e.RoomName)   // FK → Rooms(name)
                .HasPrincipalKey(r => r.Name)     // PK → Rooms(name)
                .OnDelete(DeleteBehavior.Restrict);
            }

            
    }
}