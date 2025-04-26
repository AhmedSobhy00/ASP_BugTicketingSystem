using BugTicketingSystem.BAL.DTOs.Attachments;
using BugTicketingSystem.BAL.DTOs.BugsDto;
using BugTicketingSystem.BAL.Services.Attachments;
using BugTicketingSystem.BAL.Services.Bugs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BugTicketingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BugController : ControllerBase
    {
        private readonly IBugService _bugService;
        private readonly IAttachmentService _attachmentService;

        public BugController(IBugService bugService,IAttachmentService attachmentService)
        {
            _bugService = bugService;
            _attachmentService = attachmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBugs()
        {
            var bugs = await _bugService.GetAllBugsAsync();
            return Ok(bugs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBugById(Guid id)
        {
            var result = await _bugService.GetBugByIdAsync(id);

            if (result.IsSuccess)
                return Ok(result.Data);

            return NotFound(result.Errors);
        }


        [HttpPost]

        public async Task<IActionResult> CreateBug([FromBody] BugAddDto bugDto)
        {
            if (bugDto == null)
                return BadRequest("Bug data is required.");

            var result = await _bugService.CreateBugAsync(bugDto);

            if (result.IsSuccess)
                return Ok(result.Message);

            return BadRequest(result.Errors);
        }

        [HttpPost("{id}/assignees")]
        public async Task<IActionResult> AssignUserToBug(Guid id, [FromBody] AssignUserRequest request)
        {
            var result = await _bugService.AssignUserToBugAsync(id, request.UserId);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpDelete("{id}/assignees/{userId}")]
        public async Task<IActionResult> RemoveUserFromBug(Guid id, Guid userId)
        {
            var result = await _bugService.RemoveUserFromBugAsync(id, userId);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
        ///////////
        ///

        [HttpPost("{bugId}/attachments")]
        public async Task<IActionResult> UploadAttachment(Guid bugId, [FromForm] AttachmentAddDto attachmentDto)
        {
            if (attachmentDto == null || attachmentDto.File == null)
                return BadRequest("No file uploaded.");

            var result = await _attachmentService.UploadAttachmentAsync(bugId, attachmentDto.File);

            if (result.IsSuccess)
                return Ok(new { message = result.Message });

            return BadRequest(result.Errors);
        }

        [HttpGet("{bugId}/attachments")]
        public async Task<IActionResult> GetAttachments(Guid bugId)
        {
            var attachments = await _attachmentService.GetAttachmentsForBugAsync(bugId);

            if (attachments == null)
                return NotFound("Bug not found.");

            return Ok(attachments);
        }

        [HttpDelete("{bugId}/attachments/{attachmentId}")]
        public async Task<IActionResult> DeleteAttachment(Guid bugId, Guid attachmentId)
        {
            var result = await _attachmentService.DeleteAttachmentAsync(bugId, attachmentId);

            if (result.IsSuccess)
                return Ok(result.Message);

            return BadRequest(result.Errors);
        }


    }

}
