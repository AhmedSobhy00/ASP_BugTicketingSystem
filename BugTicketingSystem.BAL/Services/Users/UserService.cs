using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BugTicketingSystem.BAL.DTOs.Common;
using BugTicketingSystem.BAL.DTOs.UserDtos;
using BugTicketingSystem.DAL.Context;
using BugTicketingSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BugTicketingSystem.BAL.Services.Users
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<GeneralResult> RegisterUserAsync(UserAddDto userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
                return GeneralResult.Failure("Username already exists.");

            var allowedRoles = new[] { "Manager", "Developer", "Tester" };
            if (string.IsNullOrEmpty(userDto.Role) || !allowedRoles.Contains(userDto.Role))
                return GeneralResult.Failure("Role must be one of the following: Manager, Developer, or Tester.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = userDto.Username,
                Role = userDto.Role
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return GeneralResult.Success("User registered successfully.");
        }


        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            return users.Select(u => new UserReadDto
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role
            });
        }

        public async Task<UserReadDto?> GetByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return null;

            return new UserReadDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<GeneralResult<string>> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
                return GeneralResult<string>.Failure("Invalid Username or password");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return GeneralResult<string>.Failure("Invalid Username or password");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]!)),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return GeneralResult<string>.Success(tokenString);
        }

    }
}
