using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.BAL.DTOs.BugsDto;
using BugTicketingSystem.BAL.DTOs.Common;
using BugTicketingSystem.DAL.Context;
using BugTicketingSystem.DAL.Models;
using BugTicketingSystem.DAL.UnitofWork;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystem.BAL.Services.Bugs
{
    public class BugService : IBugService
    {
        private readonly IUnitofwork _iunitofwork;

        public BugService(IUnitofwork unitofwork) {
            _iunitofwork = unitofwork;
        }

        public async Task<GeneralResult> CreateBugAsync(BugAddDto bugDto)
        {
            var project = await _iunitofwork.Projects.GetByIdAsync(bugDto.ProjectId);
            if (project == null)
                return GeneralResult.Failure("Project not found.");

            var bug = new Bug
            {
                Id = Guid.NewGuid(),
                Name = bugDto.Name,
                Description = bugDto.Description,
                ProjectId = bugDto.ProjectId
            };

            await _iunitofwork.Bugs.AddAsync(bug);
            await _iunitofwork.SaveChangesAsync();

            return GeneralResult.Success("Bug created successfully.");
        }

        public async Task<List<BugReadDto>> GetAllBugsAsync()
        {
            var bugs = await _iunitofwork.Bugs.GetAllAsync();

            return bugs.Select(b => new BugReadDto
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description
            }).ToList();
        }


        public async Task<GeneralResult<BugDetailsDto>> GetBugByIdAsync(Guid id)
        {
            var bug = await _iunitofwork.Bugs.GetByIdAsync(id);

            if (bug == null)
                return GeneralResult<BugDetailsDto>.Failure("Bug not found.");

            var bugDetails = new BugDetailsDto
            {
                Id = bug.Id,
                Name = bug.Name,
                Description = bug.Description,
                ProjectId = bug.ProjectId
            };

            return GeneralResult<BugDetailsDto>.Success(bugDetails);
        }


        public async Task<GeneralResult> AssignUserToBugAsync(Guid bugId, Guid userId)
        {
            var bug = await _iunitofwork.Bugs.GetByIdAsync(bugId);
            if (bug == null)
                return GeneralResult.Failure("Bug not found.");

            var user = await _iunitofwork.Users.GetByIdAsync(userId);
            if (user == null)
                return GeneralResult.Failure("User not found.");

            if (bug.Users.Any(u => u.Id == userId))
                return GeneralResult.Success("User is already assigned to this bug.");

            bug.Users.Add(user);
            await _iunitofwork.SaveChangesAsync();

            return GeneralResult.Success("User assigned to bug successfully.");
        }


        public async Task<GeneralResult> RemoveUserFromBugAsync(Guid bugId, Guid userId)
        {
            var bug = await _iunitofwork.Bugs.GetByIdAsync(bugId);
            if (bug == null)
                return GeneralResult.Failure("Bug not found.");

            var user = bug.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return GeneralResult.Success("User is not assigned to this bug."); 

            bug.Users.Remove(user);
            await _iunitofwork.SaveChangesAsync();

            return GeneralResult.Success("User unassigned from bug successfully.");
        }



    }
}
