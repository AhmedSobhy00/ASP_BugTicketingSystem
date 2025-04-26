using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.BAL.DTOs.UserDtos;
using BugTicketingSystem.DAL.Models;
using BugTicketingSystem.BAL.DTOs.Common;


namespace BugTicketingSystem.BAL.Services.Users
{
    public interface IUserService 
    {
        Task<GeneralResult> RegisterUserAsync(UserAddDto userDto);
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
        Task<UserReadDto?> GetByIdAsync(Guid id);
        Task<GeneralResult<string>> LoginAsync(LoginDto dto);


    }
}
