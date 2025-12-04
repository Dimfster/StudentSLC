using Microsoft.EntityFrameworkCore;
using StudentSLC.Data;
using StudentSLC.DTOs;
using StudentSLC.Models;

namespace StudentSLC.Services
{
    public class EventService
    {
        private readonly AppDbContext _db;

        public EventService(AppDbContext db)
        {
            _db = db;
        }

        // ---------- CREATE ----------
        public async Task<EventResponse> CreateEvent(CreateEventDTO dto)
        {
            if (await _db.Events.AnyAsync(e => e.Name == dto.Name))
                throw new Exception($"Event '{dto.Name}' already exists");

            var room = await _db.Rooms.FirstOrDefaultAsync(r => r.Name == dto.RoomName)
                ?? throw new Exception($"Room '{dto.RoomName}' not found");

            var ev = new Event
            {
                Name = dto.Name,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                RoomName = dto.RoomName,
                Room = room
            };

            // Participants
            if (dto.ParticipantIds != null)
            {
                ev.Participants = await _db.Participants
                    .Where(p => dto.ParticipantIds.Contains(p.Id))
                    .ToListAsync();
            }
            // KeyHolders
            if (dto.KeyHolderIds != null)
            {
                var keyHolders = await _db.Participants
                    .Include(p => p.User)                      
                    .Where(p => dto.KeyHolderIds.Contains(p.Id) 
                            && p.User != null
                            && p.User.Role == "keyholder")    
                    .ToListAsync();

                if (keyHolders.Count != dto.KeyHolderIds.Count)
                    throw new Exception("Some provided key holders are invalid or do not have role 'keyholder'.");

                ev.KeyHolders = keyHolders;
            }

            _db.Events.Add(ev);
            await _db.SaveChangesAsync();

            return new EventResponse
            {
                Name = ev.Name,
                StartTime = ev.StartTime,
                EndTime = ev.EndTime,
                RoomName = ev.RoomName
            };
        }

        // ---------- UPDATE ----------
        public async Task<EventResponse> UpdateEvent(string eventName, UpdateEventDTO dto)
        {
            var ev = await _db.Events
                .Include(e => e.Participants)
                .Include(e => e.KeyHolders)
                .FirstOrDefaultAsync(e => e.Name == eventName)
                ?? throw new KeyNotFoundException($"Event '{eventName}' not found");

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                // Проверка на уникальность
                if (await _db.Events.AnyAsync(e => e.Name == dto.Name))
                    throw new Exception($"Event '{dto.Name}' already exists");

                ev.Name = dto.Name;
            }

            if (dto.StartTime.HasValue)
                ev.StartTime = dto.StartTime.Value;

            if (dto.EndTime.HasValue)
                ev.EndTime = dto.EndTime.Value;

            if (!string.IsNullOrWhiteSpace(dto.RoomName))
            {
                var room = await _db.Rooms
                    .FirstOrDefaultAsync(r => r.Name == dto.RoomName)
                    ?? throw new Exception($"Room '{dto.RoomName}' not found");

                ev.RoomName = dto.RoomName;
                ev.Room = room;
            }

            // Update Participants
            if (dto.ParticipantIds != null)
            {
                ev.Participants.Clear();
                ev.Participants = await _db.Participants
                    .Where(p => dto.ParticipantIds.Contains(p.Id))
                    .ToListAsync();
            }

            // Update KeyHolders
            if (dto.KeyHolderIds != null)
            {
                ev.KeyHolders.Clear();
                ev.KeyHolders = await _db.Participants
                    .Where(p => dto.KeyHolderIds.Contains(p.Id))
                    .ToListAsync();
            }

            await _db.SaveChangesAsync();

            return new EventResponse
            {
                Name = ev.Name,
                StartTime = ev.StartTime,
                EndTime = ev.EndTime,
                RoomName = ev.RoomName
            };
        }

        // ---------- DELETE ----------
        public async Task DeleteEvent(string eventName)
        {
            var ev = await _db.Events
                .Include(e => e.Participants)
                .Include(e => e.KeyHolders)
                .FirstOrDefaultAsync(e => e.Name == eventName);

            if (ev == null)
                throw new KeyNotFoundException($"Event '{eventName}' not found");

            ev.Participants.Clear();
            ev.KeyHolders.Clear();

            _db.Events.Remove(ev);
            await _db.SaveChangesAsync();
        }
    }
}
