using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.DAL.Context;
using BugTicketingSystem.DAL.Models;
using BugTicketingSystem.DAL.Repos.Genaric;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystem.DAL.Repos.Attachments
{
    public class AttachmentRepo : GenericRepo<Attachment>, IAttachmentRepo
    {

        public AttachmentRepo(ApplicationDbContext context) : base(context)
        {
           
        }

        public async Task<List<Attachment>> GetAttachmentsForBug(Guid bugId)
        {
            var attachments = await _context.Attachments
                .Where(a => a.BugId == bugId)
                .Select(a => new Attachment
                {
                    Id = a.Id,
                    FilePath = a.FilePath
                })
                .ToListAsync();

            return attachments;
        }
    }
}
