using Microsoft.AspNetCore.Mvc;
using TT.BLL.Models;
using TT.BLL.Services;

namespace TT.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject(string name, DateTime? startTime = null, DateTime? endTime = null)
        {
            var project = await _projectService.CreateProjectAsync(name, startTime, endTime);
            return Ok(project);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/time")]
        public async Task<IActionResult> AddTime(int id, DateTime startTime, DateTime endTime)
        {
            await _projectService.AddTimeAsync(id, startTime, endTime);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _projectService.GetProjectAsync(id);
            return Ok(project);
        }
    }
}
