namespace StudentSLC.DTOs
{
    public class ScheduleDTO
    {
        public string Name { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string RoomName { get; set; } = null!;
        public List<string> KeyHolderNames { get; set; } = new();
    }
}