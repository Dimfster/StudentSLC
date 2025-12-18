using Microsoft.EntityFrameworkCore;
using StudentSLC.Data;
using StudentSLC.DTOs;
using StudentSLC.Models;

namespace StudentSLC.Services
{
    public class RoomService
    {
        private readonly AppDbContext _db;

        public RoomService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<RoomResponse> CreateRoom(CreateRoomDTO dto)
        {
            if (await _db.Rooms.AnyAsync(r => r.Name == dto.Name))
                throw new Exception($"Room '{dto.Name}' already exists");

            var room = new Room
            {
                Name = dto.Name,
                Type = dto.Type
            };

            _db.Rooms.Add(room);
            await _db.SaveChangesAsync();

            return new RoomResponse
            {
                Name = room.Name,
                Type = room.Type
            };
        }

        public async Task<RoomResponse> UpdateRoom(string roomName, UpdateRoomDTO dto)
        {
            var room = await _db.Rooms
                .FirstOrDefaultAsync(r => r.Name == roomName)
                ?? throw new KeyNotFoundException($"Room '{roomName}' not found");

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                if (await _db.Rooms.AnyAsync(r => r.Name == dto.Name))
                    throw new Exception($"Room '{dto.Name}' already exists");

                room.Name = dto.Name;
            }

            if (!string.IsNullOrWhiteSpace(dto.Type))
                room.Type = dto.Type;

            _db.Rooms.Update(room);
            await _db.SaveChangesAsync();

            return new RoomResponse
            {
                Name = room.Name,
                Type = room.Type
            };
        }

        public async Task DeleteRoom(string roomName)
        {
            var room = await _db.Rooms
                .Include(r => r.Events)
                .FirstOrDefaultAsync(r => r.Name == roomName);

            if (room == null)
                throw new KeyNotFoundException($"Room '{roomName}' not found");

            // удаляем все события в комнате
            _db.Events.RemoveRange(room.Events);

            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();
        }
        public async Task<List<RoomResponse>> GetAllRooms()
        {
            return await _db.Rooms
                .Select(r => new RoomResponse
                {
                    Name = r.Name,
                    Type = r.Type
                })
                .ToListAsync();
        }
    }
}
