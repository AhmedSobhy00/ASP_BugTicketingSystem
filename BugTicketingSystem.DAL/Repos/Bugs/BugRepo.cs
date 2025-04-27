using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.DAL.Context;
using BugTicketingSystem.DAL.Models;
using BugTicketingSystem.DAL.Repos.Genaric;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystem.DAL.Repos.Bugs
{
    public class BugRepo : GenericRepo<Bug>, IBugRepo
    {


        public BugRepo(ApplicationDbContext context):base(context)
        {
        }

        public async Task CreateAsync(Bug bug)
        {
            _context.Bugs.Add(bug);
            await _context.SaveChangesAsync();
        }

    
        public async Task AssignUserAsync(Guid bugId, Guid userId)
        {
            var bug = await _context.Bugs.Include(b => b.Users).FirstOrDefaultAsync(b => b.Id == bugId);
            if (bug != null)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null && !bug.Users.Any(u => u.Id == userId))
                {
                    bug.Users.Add(user);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveUserAsync(Guid bugId, Guid userId)
        {
            var bug = await _context.Bugs.Include(b => b.Users).FirstOrDefaultAsync(b => b.Id == bugId);
            if (bug != null)
            {
                var user = bug.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    bug.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
