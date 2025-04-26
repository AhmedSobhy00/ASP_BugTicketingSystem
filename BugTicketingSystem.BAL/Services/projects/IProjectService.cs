using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.BAL.DTOs.Common;
using BugTicketingSystem.BAL.DTOs.ProjectDto;

namespace BugTicketingSystem.BAL.Services.projects
{
    public interface IProjectService
    {
        public Task<GeneralResult<Guid>> CreateProjectAsync(ProjectAddDto projectDto);
        public Task<IEnumerable<ProjectReadDto>> GetAllProjectsAsync();
        public Task<GeneralResult<ProjectDetailsDto>> GetProjectByIdAsync(Guid id);



    }
}
