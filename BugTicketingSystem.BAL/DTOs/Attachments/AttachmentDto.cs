using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.BAL.DTOs.Attachments
{
    public class AttachmentDto
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; }
    }
}
