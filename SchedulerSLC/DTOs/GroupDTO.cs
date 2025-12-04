namespace StudentSLC.DTOs
{
    public class CreateGroupDTO
    {
        public string Name { get; set; } = null!;
    }

    public class UpdateGroupDTO
    {
        public string Name { get; set; } = null!;
    }

    public class GroupResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
