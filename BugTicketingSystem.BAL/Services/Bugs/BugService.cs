using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.BAL.DTOs.BugsDto;
using BugTicketingSystem.BAL.DTOs.Common;
using BugTicketingSystem.DAL.Context;
using BugTicketingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystem.BAL.Services.Bugs
{
    public class BugService : IBugService
    {
        private readonly ApplicationDbContext _context;

        public BugService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<GeneralResult> CreateBugAsync(BugAddDto bugDto)
        {
            var project = await _context.Projects.FindAsync(bugDto.ProjectId);
            if (project == null)
                return GeneralResult.Failure("Project not found.");

            var bug = new Bug
            {
                Id = Guid.NewGuid(),
                Name = bugDto.Name,
                Description = bugDto.Description,
                ProjectId = bugDto.ProjectId
            };

            _context.Bugs.Add(bug);
            await _context.SaveChangesAsync();

            return GeneralResult.Success("Bug created successfully.");
        }

        public async Task<List<BugReadDto>> GetAllBugsAsync()
        {
            return await _context.Bugs
                .Select(b => new BugReadDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description
                })
                .ToListAsync();
        }

        public async Task<GeneralResult<BugDetailsDto>> GetBugByIdAsync(Guid id)
        {
            var bug = await _context.Bugs.FindAsync(id);

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
            var bug = await _context.Bugs.Include(b => b.Users)
                .FirstOrDefaultAsync(b => b.Id == bugId);
            if (bug == null)
                return GeneralResult.Failure("Bug not found.");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return GeneralResult.Failure("User not found.");

            if (bug.Users.Any(u => u.Id == userId))
                return GeneralResult.Success("User is already assigned to this bug."); 

            bug.Users.Add(user);
            await _context.SaveChangesAsync();

            return GeneralResult.Success("User assigned to bug successfully.");
        }

        public async Task<GeneralResult> RemoveUserFromBugAsync(Guid bugId, Guid userId)
        {
            var bug = await _context.Bugs.Include(b => b.Users)
                .FirstOrDefaultAsync(b => b.Id == bugId);
            if (bug == null)
                return GeneralResult.Failure("Bug not found.");

            var user = bug.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return GeneralResult.Success("User is not assigned to this bug."); 

            bug.Users.Remove(user);
            await _context.SaveChangesAsync();

            return GeneralResult.Success("User unassigned from bug successfully.");
        }



    }
}
