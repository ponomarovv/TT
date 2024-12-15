namespace TT.DAL.Entities
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Project Project { get; set; } = null!;
    }
}
