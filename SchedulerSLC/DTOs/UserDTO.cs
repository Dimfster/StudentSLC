namespace StudentSLC.DTOs
{
    public class CreateUserDTO
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string Role { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
    public class UpdateUserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public string? Role { get; set; }
        public string? Password { get; set; }
        public List<string>? GroupNames { get; set; }
    }
    public class UserResponse
    {
        public int? UserCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
        public string? Role { get; set; }
    }
}