using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TT.BLL.Exceptions;
using TT.BLL.Models;
using TT.DAL;
using TT.DAL.Entities;

namespace TT.BLL.Services;

public class ProjectService : IProjectService
{
    private readonly TTDbContext _dbContext;
    private readonly IMapper _mapper;

    public ProjectService(TTDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ProjectModel> CreateProjectAsync(string name, DateTime? startTime = null,
        DateTime? endTime = null)
    {
        var project = new Project { Name = name, StartTime = startTime, EndTime = endTime };
        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<ProjectModel>(project);
    }

    public async Task<ProjectModel> CompleteProjectAsync(int id)
    {
        var project = await _dbContext.Projects.FindAsync(id);

        if (project == null)
        {
            throw new NotFoundException("Project not found.");
        }

        if (project.IsCompleted)
        {
            throw new BadRequestException("Project is already completed.");
        }

        project.IsCompleted = true;
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ProjectModel>(project);
    }

    public async Task DeleteProjectAsync(int projectId)
    {
        var project = await _dbContext.Projects.FindAsync(projectId);
        if (project == null)
        {
            throw new NotFoundException("Project not found.");
        }

        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
    }


    public async Task<ProjectModel> GetProjectAsync(int projectId)
    {
        var project = await _dbContext.Projects.Include(p => p.TimeEntries).FirstOrDefaultAsync(p => p.Id == projectId);
        if (project == null) throw new NotFoundException("Project not found.");

        var model = _mapper.Map<ProjectModel>(project);
        
        double totalTimeSpent = project.TimeEntries
            .Sum(te => (te.EndTime - te.StartTime).TotalHours);
        
        model.TotalTimeSpent = totalTimeSpent;
        return model;
    }
}
