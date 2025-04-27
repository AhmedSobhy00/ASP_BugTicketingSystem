using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.BAL.DTOs.Attachments;
using BugTicketingSystem.BAL.DTOs.Common;
using BugTicketingSystem.DAL.Context;
using BugTicketingSystem.DAL.Models;
using BugTicketingSystem.DAL.UnitofWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystem.BAL.Services.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IUnitofwork _unitofwork;

        public AttachmentService(IUnitofwork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public async Task<GeneralResult> UploadAttachmentAsync(Guid bugId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return GeneralResult.Failure("No file uploaded.");

            var bug = await _unitofwork.Bugs.GetByIdAsync(bugId);
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

            await _unitofwork.Attachments.AddAsync(attachment);
            await _unitofwork.SaveChangesAsync();

            return GeneralResult.Success("Attachment uploaded successfully.");
        }

        public async Task<List<AttachmentDto>> GetAttachmentsForBugAsync(Guid bugId)
        {
            var attachments = await _unitofwork.Attachments
                .GetQueryable()
                .Where(a => a.BugId == bugId)
                .Select(a => new AttachmentDto
                {
                    Id = a.Id,
                    FilePath = a.FilePath
                })
                .ToListAsync();

            return attachments;
        }

        public async Task<GeneralResult> DeleteAttachmentAsync(Guid bugId, Guid attachmentId)
        {
            var attachment = await _unitofwork.Attachments.SingleOrDefaultAsync(a => a.BugId == bugId && a.Id == attachmentId);

            if (attachment == null)
                return GeneralResult.Failure("Attachment not found for the specified bug.");

            if (System.IO.File.Exists(attachment.FilePath))
            {
                System.IO.File.Delete(attachment.FilePath);
            }

            _unitofwork.Attachments.Delete(attachment);
            await _unitofwork.SaveChangesAsync();

            return GeneralResult.Success("Attachment deleted successfully.");
        }
    }
}
