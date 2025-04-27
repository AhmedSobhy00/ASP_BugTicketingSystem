using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.BAL.DTOs.BugsDto;
using BugTicketingSystem.BAL.DTOs.Common;

namespace BugTicketingSystem.BAL.Services.Bugs
{
    public interface IBugService
    {
        Task<GeneralResult> CreateBugAsync(BugAddDto bugDto);
        Task<List<BugReadDto>> GetAllBugsAsync();
        Task<GeneralResult<BugDetailsDto>> GetBugByIdAsync(Guid id);


        Task<GeneralResult> AssignUserToBugAsync(Guid bugId, Guid userId);
        Task<GeneralResult> RemoveUserFromBugAsync(Guid bugId, Guid userId);


    }
}
