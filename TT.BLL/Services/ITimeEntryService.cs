using TT.BLL.Models;

namespace TT.BLL.Services;

public interface ITimeEntryService
{
    Task<TimeEntryDTO> CreateTimeEntryAsync(CreateTimeEntryDTO model);
    Task<List<TimeEntryDTO>> GetTimeEntriesByProjectIdAsync(int projectId);
    Task DeleteTimeEntryAsync(int timeEntryId);
}