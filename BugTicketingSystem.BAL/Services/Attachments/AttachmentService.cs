using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.BAL.DTOs.Attachments;
using BugTicketingSystem.BAL.DTOs.Common;
using BugTicketingSystem.DAL.Context;
using BugTicketingSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystem.BAL.Services.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        private readonly ApplicationDbContext _context;

        public AttachmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GeneralResult> UploadAttachmentAsync(Guid bugId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return GeneralResult.Failure("No file uploaded.");

            var bug = await _context.Bugs.FindAsync(bugId);
            if (bug == null)
                return GeneralResult.Failure("Bug not found.");

            var filePath = Path.Combine("uploads", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new Attachment
            {
                Id = Guid.NewGuid(),
                FilePath = filePath,
                BugId = bugId
            };

            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();

            return GeneralResult.Success("Attachment uploaded successfully.");
        }

        public async Task<List<AttachmentDto>> GetAttachmentsForBugAsync(Guid bugId)
        {
            var bug = await _context.Bugs.Include(b => b.Attachments)
                .FirstOrDefaultAsync(b => b.Id == bugId);

            if (bug == null)
                return null;

            return bug.Attachments.Select(a => new AttachmentDto
            {
                Id = a.Id,
                FilePath = a.FilePath
            }).ToList();
        }

        public async Task<GeneralResult> DeleteAttachmentAsync(Guid bugId, Guid attachmentId)
        {
            var bug = await _context.Bugs.Include(b => b.Attachments)
                .FirstOrDefaultAsync(b => b.Id == bugId);

            if (bug == null)
                return GeneralResult.Failure("Bug not found.");

            var attachment = bug.Attachments.FirstOrDefault(a => a.Id == attachmentId);
            if (attachment == null)
                return GeneralResult.Failure("Attachment not found.");

            if (System.IO.File.Exists(attachment.FilePath))
            {
                System.IO.File.Delete(attachment.FilePath);
            }

            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();

            return GeneralResult.Success("Attachment deleted successfully.");
        }
    }
}
