using Microsoft.EntityFrameworkCore;
using StudentSLC.DTOs;
using StudentSLC.Models;
using StudentSLC.Data;
using StudentSLC.Security;

namespace StudentSLC.Services
{
    public class UserService
    {
        private readonly AppDbContext _db;
        private readonly PasswordHasher _passwordHasher;
         private readonly CodeGenerator _codeGenerator;

        public UserService(AppDbContext db, PasswordHasher passwordHasher, CodeGenerator codeGenerator)
        {
            _db = db;
            _passwordHasher = passwordHasher;
            _codeGenerator = codeGenerator;
        }

        public async Task<UserResponse> CreateUser(CreateUserDTO dto)
        {
            // Проверка роли
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == dto.Role) 
                ?? throw new KeyNotFoundException($"Role '{dto.Role}' not found");

            var participant = new Participant
            {
                Type = "user"
            };

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Patronymic = dto.Patronymic,
                Role = dto.Role,
                UserCode = await _codeGenerator.GenerateUserCode(),
                PasswordHash = _passwordHasher.Generate(dto.Password),
                Participant = participant
            };

            var entry = _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return new UserResponse
            {
                UserCode = user.UserCode,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Patronymic = user.Patronymic,
                Role = user.Role
            };
        }
        public async Task<UserResponse> UpdateUser(int userCode, UpdateUserDTO dto)
        {
            var user = await _db.Users
                .Include(u => u.Participant)
                .FirstOrDefaultAsync(u => u.UserCode == userCode) 
                ?? throw new KeyNotFoundException($"User with code {userCode} not found");

            // Проверяем роль
            var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == dto.Role);
            if (role == null)
                throw new KeyNotFoundException($"Role '{dto.Role}' not found");

            // Обновляем данные
            if (!string.IsNullOrWhiteSpace(dto.FirstName))
                user.FirstName = dto.FirstName;
            if (!string.IsNullOrWhiteSpace(dto.LastName))
                user.LastName = dto.LastName;
            if (!string.IsNullOrWhiteSpace(dto.Patronymic))
                user.Patronymic = dto.Patronymic;
            if (!string.IsNullOrWhiteSpace(dto.Role))
                user.Role = dto.Role;
            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.PasswordHash = _passwordHasher.Generate(dto.Password);

            if (dto.GroupNames != null)
            {
                var groups = await _db.Groups
                    .Where(g => dto.GroupNames.Contains(g.Name))
                    .ToListAsync();

                var missing = dto.GroupNames.Except(groups.Select(g => g.Name)).ToList();
                if (missing.Count != 0)
                    throw new KeyNotFoundException($"Groups not found: {string.Join(", ", missing)}");

                user.Groups.Clear();
                foreach (var g in groups)
                    user.Groups.Add(g);
            }

            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return new UserResponse
            {
                UserCode = user.UserCode,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Patronymic = user.Patronymic,
                Role = user.Role
            };
        }
        public async Task DeleteUser(int userCode)
        {
            var user = await _db.Users
                .Include(u => u.Participant)
                .Include(u => u.Groups)
                .FirstOrDefaultAsync(u => u.UserCode == userCode);

            if (user == null)
                throw new KeyNotFoundException($"User with code {userCode} not found");

            user.Groups.Clear();

            if (user.Participant != null)
                _db.Participants.Remove(user.Participant);

            _db.Users.Remove(user);

            await _db.SaveChangesAsync();
        }
    }
}