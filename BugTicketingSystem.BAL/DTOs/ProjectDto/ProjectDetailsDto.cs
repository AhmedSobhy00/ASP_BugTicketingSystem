using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.BAL.DTOs.BugsDto;

namespace BugTicketingSystem.BAL.DTOs.ProjectDto
{
    public class ProjectDetailsDto
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<BugReadDto> Bugs { get; set; } = new();
    }
}
