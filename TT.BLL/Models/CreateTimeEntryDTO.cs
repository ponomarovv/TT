namespace TT.BLL.Models;

public class CreateTimeEntryDTO
{
    public int ProjectId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}