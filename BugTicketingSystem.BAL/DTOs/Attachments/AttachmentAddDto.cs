using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BugTicketingSystem.BAL.DTOs.Attachments
{
    public class AttachmentAddDto
    {
        public IFormFile File { get; set; }

    }
}
