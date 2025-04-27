using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.DAL.Context;
using BugTicketingSystem.DAL.Models;
using BugTicketingSystem.DAL.Repos.Genaric;

namespace BugTicketingSystem.DAL.Repos.Projects
{
    public class ProjectsRepo : GenericRepo<Project>, IProjectsRepo
    {
        public ProjectsRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}
