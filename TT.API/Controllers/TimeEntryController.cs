using Microsoft.AspNetCore.Mvc;
using TT.BLL.Models;
using TT.BLL.Services;

namespace TT.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeEntryController : ControllerBase
{
    private readonly ITimeEntryService _timeEntryService;

    public TimeEntryController(ITimeEntryService timeEntryService)
    {
        _timeEntryService = timeEntryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTimeEntry([FromBody] CreateTimeEntryDTO model)
    {
        var timeEntry = await _timeEntryService.CreateTimeEntryAsync(model);
        return CreatedAtAction(nameof(GetTimeEntriesByProjectId), new { projectId = timeEntry.ProjectId }, timeEntry);
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> GetTimeEntriesByProjectId(int projectId)
    {
        var timeEntries = await _timeEntryService.GetTimeEntriesByProjectIdAsync(projectId);
        return Ok(timeEntries);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTimeEntry(int id)
    {
        await _timeEntryService.DeleteTimeEntryAsync(id);
        return NoContent();
    }
}
