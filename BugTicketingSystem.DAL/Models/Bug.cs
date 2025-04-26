using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DAL.Models
{
    public class Bug
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
