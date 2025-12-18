namespace StudentSLC.DTOs
{
    public class RegisterRequestDTO
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Password { get; set; }  = null!;
    }
    public class RegisterResponseDTO
    {
        public int UserCode { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Role { get; set; } = null!;
    }
    public class LoginRequestDTO
    {
        public int UserCode { get; set; }
        public string Password { get; set; }  = null!;
    }

    public class LoginResponseDTO
    {
        public int UserCode { get; set; }
        public required string Role { get; set; }
        public required string Token { get; set; }
        public int ExpiresIn { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}