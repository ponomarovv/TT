namespace TT.BLL.Models;

public class TimeEntryDTO
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}