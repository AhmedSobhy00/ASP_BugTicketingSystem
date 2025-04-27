using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.BAL.DTOs.BugsDto;
using BugTicketingSystem.BAL.DTOs.Common;
using BugTicketingSystem.BAL.DTOs.ProjectDto;
using BugTicketingSystem.DAL.Context;
using BugTicketingSystem.DAL.Models;
using BugTicketingSystem.DAL.UnitofWork;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystem.BAL.Services.projects
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitofwork _unitofwork;

        public ProjectService(IUnitofwork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task<GeneralResult<Guid>> CreateProjectAsync(ProjectAddDto projectDto)
        {
            var existingProject = await _unitofwork.Projects
                .AnyAsync(p => p.ProjectName == projectDto.ProjectName);

            if (existingProject)
            {
                return GeneralResult<Guid>.Failure("Project name already exists.");
            }

            var project = new Project
            {
                Id = Guid.NewGuid(),
                ProjectName = projectDto.ProjectName,
                Description = projectDto.Description
            };

            await _unitofwork.Projects.AddAsync(project);
            await _unitofwork.SaveChangesAsync();

            return GeneralResult<Guid>.Success(project.Id);
        }


        public async Task<IEnumerable<ProjectReadDto>> GetAllProjectsAsync()
        {
            var projects = await _unitofwork.Projects.GetAllAsync();

            return projects.Select(p => new ProjectReadDto
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                Description = p.Description
            });
        }


        public async Task<GeneralResult<ProjectDetailsDto>> GetProjectByIdAsync(Guid id)
        {
            var project = await _unitofwork.Projects
              .GetQueryable()
              .Include(p => p.Bugs)
              .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return GeneralResult<ProjectDetailsDto>.Failure("Project not found.");

            var projectDetails = new ProjectDetailsDto
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Description = project.Description,
                Bugs = project.Bugs.Select(b => new BugReadDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description
                }).ToList()
            };

            return GeneralResult<ProjectDetailsDto>.Success(projectDetails);
        }
    }
}
