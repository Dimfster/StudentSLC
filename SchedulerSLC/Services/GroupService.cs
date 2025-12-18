using Microsoft.EntityFrameworkCore;
using StudentSLC.Data;
using StudentSLC.DTOs;
using StudentSLC.Models;

namespace StudentSLC.Services
{
    public class GroupService
    {
        private readonly AppDbContext _db;

        public GroupService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<GroupResponse> CreateGroup(CreateGroupDTO dto)
        {
            if (await _db.Groups.AnyAsync(g => g.Name == dto.Name))
                throw new Exception($"Group '{dto.Name}' already exists");

            var participant = new Participant { Type = "group" };

            var group = new Group
            {
                Name = dto.Name,
                Participant = participant
            };

            _db.Groups.Add(group);
            await _db.SaveChangesAsync();

            return new GroupResponse
            {
                Id = group.Id,
                Name = group.Name
            };
        }

        public async Task<GroupResponse> UpdateGroup(string groupName, UpdateGroupDTO dto)
        {
            var group = await _db.Groups
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Name == groupName)
                ?? throw new KeyNotFoundException($"Group '{groupName}' not found");

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                if (await _db.Groups.AnyAsync(g => g.Name == dto.Name))
                    throw new Exception($"Group '{dto.Name}' already exists");

                group.Name = dto.Name;
            }

            _db.Groups.Update(group);
            await _db.SaveChangesAsync();

            return new GroupResponse
            {
                Id = group.Id,
                Name = group.Name
            };
        }

        public async Task DeleteGroup(string groupName)
        {
            var group = await _db.Groups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Name == groupName);

            if (group == null)
                throw new KeyNotFoundException($"Group '{groupName}' not found");

            foreach (var user in group.Users)
            {
                user.Groups.Remove(group);
            }

            _db.Groups.Remove(group);
            await _db.SaveChangesAsync();
        }

        public async Task<List<GroupResponse>> GetAllGroups()
        {
            return await _db.Groups
                .Select(g => new GroupResponse
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToListAsync();
        }
    }
}
