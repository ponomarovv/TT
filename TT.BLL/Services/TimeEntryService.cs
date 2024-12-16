using AutoMapper;
using TT.BLL.Models;
using TT.DAL;
using TT.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using TT.BLL.Exceptions;

namespace TT.BLL.Services;

public class TimeEntryService : ITimeEntryService
{
    private readonly TTDbContext _dbContext;
    private readonly IMapper _mapper;

    public TimeEntryService(TTDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<TimeEntryDTO> CreateTimeEntryAsync(CreateTimeEntryDTO model)
    {
        Project? project = await _dbContext.Projects
            .Where(p => p.Id == model.ProjectId)
            .Include(p => p.TimeEntries)
            .FirstOrDefaultAsync();

        #region error handling

        if (project == null)
        {
            throw new NotFoundException("Project not found");
        }

        if (project.IsCompleted == true)
        {
            throw new BadRequestException("Project is completed");
        }

        // Bonus: Projects can have start and end time. Validate that time added to project fits.
        if (project.StartTime > model.StartTime || project.EndTime < model.EndTime
                                                || model.StartTime > project.EndTime ||
                                                model.EndTime < project.StartTime)
        {
            throw new BadRequestException("Time entry does not fit within the project's start and end time.");
        }

        if (model.StartTime > model.EndTime)
        {
            throw new BadRequestException("Time entry's StartTime should be less than the EndTime.");
        }

        if ((model.EndTime - model.StartTime).TotalMinutes < 15)
        {
            throw new BadRequestException("Minimum time entry duration is 15 minutes.");
        }

        #endregion

        var timeEntry = _mapper.Map<TimeEntry>(model);
        
        #region Bonus: Overlapping times can be merged
        
        var timeEntryBefore = project.TimeEntries.FirstOrDefault(e => e.EndTime == model.StartTime);
        var timeEntryAfter = project.TimeEntries.FirstOrDefault(e => e.StartTime == model.EndTime);
        
        if (timeEntryBefore != null && timeEntryAfter != null)
        {
            await DeleteTimeEntryAsync(timeEntryAfter.Id);
            
            timeEntryBefore.EndTime=timeEntryAfter.EndTime;
            
            _dbContext.TimeEntries.Update(timeEntryBefore);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TimeEntryDTO>(timeEntryBefore);
        }

        if (timeEntryBefore != null && timeEntryAfter == null)
        {
            timeEntryBefore.EndTime = model.EndTime;

            _dbContext.TimeEntries.Update(timeEntryBefore);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TimeEntryDTO>(timeEntryBefore);
        }

        if (timeEntryBefore == null && timeEntryAfter != null)
        {
            timeEntryAfter.StartTime = model.StartTime;

            _dbContext.TimeEntries.Update(timeEntryAfter);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TimeEntryDTO>(timeEntryAfter);
        }
        #endregion

        await _dbContext.TimeEntries.AddAsync(timeEntry);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<TimeEntryDTO>(timeEntry);
    }

    public async Task<List<TimeEntryDTO>> GetTimeEntriesByProjectIdAsync(int projectId)
    {
        var timeEntries = await _dbContext.TimeEntries
            .Where(te => te.ProjectId == projectId)
            .ToListAsync();

        return _mapper.Map<List<TimeEntryDTO>>(timeEntries);
    }

    public async Task DeleteTimeEntryAsync(int timeEntryId)
    {
        var timeEntry = await _dbContext.TimeEntries.FindAsync(timeEntryId);
        if (timeEntry == null)
        {
            throw new NotFoundException("TimeEntry not found.");
        }

        _dbContext.TimeEntries.Remove(timeEntry);
        await _dbContext.SaveChangesAsync();
    }
}
