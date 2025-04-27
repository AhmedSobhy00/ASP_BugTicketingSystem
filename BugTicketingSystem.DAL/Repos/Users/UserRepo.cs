using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.DAL.Context;
using BugTicketingSystem.DAL.Models;
using BugTicketingSystem.DAL.Repos.Genaric;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystem.DAL.Repos.Users
{
    public class UserRepo : GenericRepo<User>, IUserRepo
    {

        public UserRepo(ApplicationDbContext context) : base(context)
        {
        }
    }
}
