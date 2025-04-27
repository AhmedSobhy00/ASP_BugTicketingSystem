using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.DAL.Models;
using BugTicketingSystem.DAL.Repos.Genaric;

namespace BugTicketingSystem.DAL.Repos.Bugs
{
    public interface IBugRepo : IGenericRepo<Bug>
    {
        Task AssignUserAsync(Guid bugId, Guid userId); 
        Task RemoveUserAsync(Guid bugId, Guid userId); 
    }
}
