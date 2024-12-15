﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TT.BLL.Models;
using TT.DAL;
using TT.DAL.Entities;

namespace TT.BLL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly TTDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProjectService(TTDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProjectModel> CreateProjectAsync(string name, DateTime? startTime = null, DateTime? endTime = null)
        {
            var project = new Project { Name = name, StartTime = startTime, EndTime = endTime };
            _dbContext.Projects.Add(project);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<ProjectModel>(project);
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            var project = await _dbContext.Projects.FindAsync(projectId);
            if (project == null) throw new KeyNotFoundException("Project not found.");
            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddTimeAsync(int projectId, DateTime startTime, DateTime endTime)
        {
            var project = await _dbContext.Projects.Include(p => p.TimeEntries).FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null) throw new KeyNotFoundException("Project not found.");
            if (project.IsCompleted) throw new InvalidOperationException("Cannot add time to a completed project.");
            if (project.StartTime.HasValue && startTime < project.StartTime || project.EndTime.HasValue && endTime > project.EndTime)
                throw new InvalidOperationException("Time entry does not fit within project bounds.");

            project.TimeEntries.Add(new TimeEntry { StartTime = startTime, EndTime = endTime });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ProjectModel> GetProjectAsync(int projectId)
        {
            var project = await _dbContext.Projects.Include(p => p.TimeEntries).FirstOrDefaultAsync(p => p.Id == projectId);
            if (project == null) throw new KeyNotFoundException("Project not found.");

            var model = _mapper.Map<ProjectModel>(project);
            model.TotalTimeSpent = project.TimeEntries
                .Aggregate(TimeSpan.Zero, (sum, entry) => sum + (entry.EndTime - entry.StartTime));
            return model;
        }
    }
}