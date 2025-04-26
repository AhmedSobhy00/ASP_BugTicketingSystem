using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.BAL.DTOs.Attachments;
using BugTicketingSystem.BAL.DTOs.Common;
using Microsoft.AspNetCore.Http;

namespace BugTicketingSystem.BAL.Services.Attachments
{
    public interface IAttachmentService
    {
        public Task<GeneralResult> UploadAttachmentAsync(Guid bugId, IFormFile file);
        public Task<List<AttachmentDto>> GetAttachmentsForBugAsync(Guid bugId);
        public Task<GeneralResult> DeleteAttachmentAsync(Guid bugId, Guid attachmentId);



    }
}
