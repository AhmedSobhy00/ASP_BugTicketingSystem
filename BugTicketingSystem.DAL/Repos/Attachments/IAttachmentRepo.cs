using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.DAL.Models;
using BugTicketingSystem.DAL.Repos.Genaric;

namespace BugTicketingSystem.DAL.Repos.Attachments
{
    public interface IAttachmentRepo : IGenericRepo<Attachment>
    {
        Task<List<Attachment>> GetAttachmentsForBug(Guid bugId);

    }
}
