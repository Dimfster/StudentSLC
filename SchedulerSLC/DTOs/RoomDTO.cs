namespace StudentSLC.DTOs
{
    public class CreateRoomDTO
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }

    public class UpdateRoomDTO
    {
        public string? Name { get; set; }
        public string? Type { get; set; }
    }

    public class RoomResponse
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
