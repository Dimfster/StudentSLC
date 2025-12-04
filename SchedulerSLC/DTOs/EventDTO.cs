namespace StudentSLC.DTOs
{
    public class CreateEventDTO
    {
        public string Name { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string RoomName { get; set; } = null!;

        public List<Guid>? ParticipantIds { get; set; }

        public List<Guid>? KeyHolderIds { get; set; }
    }

    public class UpdateEventDTO
    {
        public string? Name { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string? RoomName { get; set; }

        public List<Guid>? ParticipantIds { get; set; }

        public List<Guid>? KeyHolderIds { get; set; }
    }

    public class EventResponse
    {
        public string Name { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string RoomName { get; set; } = null!;
    }
}
