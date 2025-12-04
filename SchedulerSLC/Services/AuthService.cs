using Microsoft.EntityFrameworkCore;
using StudentSLC.DTOs;
using StudentSLC.Models;
using StudentSLC.Data;
using StudentSLC.Security;

namespace StudentSLC.Services
{
    public class AuthService
    {
        private readonly AppDbContext _db;
        private readonly PasswordHasher _passwordHasher;
        private readonly JwtProvider _jwtProvider;
        private readonly Random _random = new Random();

        public AuthService(AppDbContext db, PasswordHasher passwordHasher, JwtProvider jwtProvider)
        {
            _db = db;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        private async Task<int> GenerateUserCode()
        {
            int code;
            bool exists;

            do
            {
                code = _random.Next(100000, 999999);
                exists = await _db.Users.AnyAsync(u => u.UserCode == code);
            }
            while (exists);

            return code;
        }
        public async Task<RegisterResponseDTO> Register(RegisterRequestDTO regiserDTO)
        {
            var participant = new Participant { Type = "user" };

            var user = new User
            {
                UserCode = await GenerateUserCode(),
                FirstName = regiserDTO.FirstName,
                LastName = regiserDTO.LastName,
                Patronymic = regiserDTO.Patronymic,
                Role = "student",
                PasswordHash = _passwordHasher.Generate(regiserDTO.Password),
                Participant = participant
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new RegisterResponseDTO
            {
                UserCode = user.UserCode,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Patronymic = user.Patronymic,
                Role = user.Role
            };
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginDTO)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserCode == loginDTO.UserCode);
            if (user == null || !_passwordHasher.Verify(loginDTO.Password, user.PasswordHash))
                throw new Exception("Неверный email или пароль");

            var token = _jwtProvider.GenerateToken(user);

            return new LoginResponseDTO
            {
                UserCode = user.UserCode,
                Role = user.Role,
                Token = token,
                ExpiresIn = 3600
            };
        }
    }
}