using BugTicketingSystem.BAL.DTOs.ProjectDto;
using BugTicketingSystem.BAL.Services.projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BugTicketingSystem.API.Controllers
{
    [Route("api/projects")]
    [ApiController]
    [Authorize] 
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        [Authorize(Policy = "ManagerPolicy")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectAddDto projectDto)
        {
            if (projectDto == null)
                return BadRequest(new { Message = "Project data is required." });

            var result = await _projectService.CreateProjectAsync(projectDto);

            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(GetProjectById), new { id = result.Data }, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var result = await _projectService.GetProjectByIdAsync(id);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Errors);
        }

    }
}
