using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.DAL.Context;
using BugTicketingSystem.DAL.Models;
using BugTicketingSystem.DAL.Repos.Genaric;

namespace BugTicketingSystem.DAL.UnitofWork
{
    public class Unitofwork : IUnitofwork
    {
        private readonly ApplicationDbContext _context;

        public IGenericRepo<Bug> Bugs { get; private set; }
        public IGenericRepo<User> Users { get; private set; }
        public IGenericRepo<Project> Projects { get; private set; }
        public IGenericRepo<Attachment> Attachments { get; private set; }

        public Unitofwork(ApplicationDbContext context)
        {
            _context = context;
            Bugs = new GenericRepo<Bug>(_context);
            Users = new GenericRepo<User>(_context);
            Projects = new GenericRepo<Project>(_context);
            Attachments = new GenericRepo<Attachment>(_context);
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
