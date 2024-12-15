using TT.BLL.Models;

namespace TT.BLL.Services;

public interface IProjectService
{
    Task<ProjectModel> CreateProjectAsync(string name, DateTime? startTime = null, DateTime? endTime = null);
    Task<ProjectModel> CompleteProjectAsync(int id);
    Task DeleteProjectAsync(int projectId);
    Task<ProjectModel> GetProjectAsync(int projectId);
}
