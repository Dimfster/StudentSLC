using Microsoft.EntityFrameworkCore;
using StudentSLC.Data;
using StudentSLC.DTOs;
using StudentSLC.Models;

namespace StudentSLC.Services
{
    public class ScheduleService
    {
        private readonly AppDbContext _db;

        public ScheduleService(AppDbContext db)
        {
            _db = db;
        }

        private DateTime StartOfWeek(DateTime date)
        {
            var diff = date.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0) diff += 7;
            return date.AddDays(-diff).Date;
        }

        private DateTime EndOfWeek(DateTime date)
        {
            return StartOfWeek(date).AddDays(7).AddTicks(-1);
        }

        public async Task<List<ScheduleDTO>> GetGroupSchedule(string groupName, DateTime weekStart)
        {
            var group = await _db.Groups
                .Include(g => g.Users)
                    .ThenInclude(u => u.Participant)
                        .ThenInclude(p => p.EventsAsParticipant)
                            .ThenInclude(e => e.KeyHolders)
                                .ThenInclude(k => k.User)
                .Include(g => g.Users)
                    .ThenInclude(u => u.Participant)
                        .ThenInclude(p => p.EventsAsParticipant)
                            .ThenInclude(e => e.Room)
                .FirstOrDefaultAsync(g => g.Name == groupName);

            if (group == null)
                throw new KeyNotFoundException($"Group '{groupName}' not found");

            var start = StartOfWeek(weekStart);
            var end = EndOfWeek(weekStart);

            return group.Users
                .SelectMany(u => u.Participant.EventsAsParticipant)
                .Where(e => e.StartTime >= start && e.StartTime <= end)
                .Distinct()
                .Select(e => new ScheduleDTO
                {
                    Name = e.Name,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    RoomName = e.RoomName,
                    KeyHolderNames = e.KeyHolders
                        .Select(k => k.User!.FirstName + " " + k.User!.LastName)
                        .ToList()
                }).ToList();
        }

        public async Task<List<ScheduleDTO>> GetKeyHolderSchedule(int userCode, DateTime weekStart)
        {
            var participant = await _db.Participants
                .Include(p => p.EventsAsKeyHolder)
                    .ThenInclude(e => e.Room)
                .Include(p => p.EventsAsKeyHolder)
                    .ThenInclude(e => e.KeyHolders)
                        .ThenInclude(k => k.User)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.User!.UserCode == userCode);

            if (participant == null || participant.User!.Role != "keyholder")
                throw new KeyNotFoundException($"Keyholder with code {userCode} not found");

            var start = StartOfWeek(weekStart);
            var end = EndOfWeek(weekStart);

            return participant.EventsAsKeyHolder
                .Where(e => e.StartTime >= start && e.StartTime <= end)
                .Select(e => new ScheduleDTO
                {
                    Name = e.Name,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    RoomName = e.RoomName,
                    KeyHolderNames = e.KeyHolders
                        .Select(k => k.User!.FirstName + " " + k.User!.LastName)
                        .ToList()
                }).ToList();
        }

        public async Task<List<ScheduleDTO>> GetRoomSchedule(string roomName, DateTime weekStart)
        {
            var room = await _db.Rooms
                .FirstOrDefaultAsync(r => r.Name == roomName)
                ?? throw new KeyNotFoundException($"Room with name {roomName} not found");

            var start = DateTime.SpecifyKind(StartOfWeek(weekStart), DateTimeKind.Utc);
            var end = DateTime.SpecifyKind(EndOfWeek(weekStart), DateTimeKind.Utc);

            var events = await _db.Events
                .Include(e => e.Room)
                .Include(e => e.KeyHolders)
                    .ThenInclude(k => k.User)
                .Where(e => e.RoomName == roomName && e.StartTime >= start && e.StartTime <= end)
                .ToListAsync();

            return events.Select(e => new ScheduleDTO
            {
                Name = e.Name,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                RoomName = e.RoomName,
                KeyHolderNames = e.KeyHolders
                    .Select(k => k.User!.FirstName + " " + k.User!.LastName)
                    .ToList()
            }).ToList();
        }
    }
}
