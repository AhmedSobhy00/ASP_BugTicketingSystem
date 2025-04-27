using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.DAL.Models;
using BugTicketingSystem.DAL.Repos.Genaric;

namespace BugTicketingSystem.DAL.UnitofWork
{
    public interface IUnitofwork
    {
        IGenericRepo<Bug> Bugs { get; }
        IGenericRepo<User> Users { get; }
        IGenericRepo<Project> Projects { get; }
        IGenericRepo<Attachment> Attachments { get; }

        Task<int> SaveChangesAsync();
    }
}
