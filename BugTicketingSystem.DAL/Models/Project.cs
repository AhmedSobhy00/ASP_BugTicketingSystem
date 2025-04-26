using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DAL.Models
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ProjectName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        public ICollection<Bug> Bugs { get; set; } = new List<Bug>();
    }
}
