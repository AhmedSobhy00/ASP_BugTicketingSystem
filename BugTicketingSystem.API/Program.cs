using System.Text;
using BugTicketingSystem.BAL.Services.Attachments;
using BugTicketingSystem.BAL.Services.Bugs;
using BugTicketingSystem.BAL.Services.projects;
using BugTicketingSystem.BAL.Services.Users;
using BugTicketingSystem.DAL.Context;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BugTicketingSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("default"))
            );
            builder.Services.AddScoped<IProjectService, ProjectService>();

            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IBugService, BugService>();

            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 104857600; 
            });

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
                options.AddPolicy("DeveloperPolicy", policy => policy.RequireRole("Developer"));
                options.AddPolicy("TesterPolicy", policy => policy.RequireRole("Tester"));
            });



            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseAuthentication(); 
            app.UseAuthorization();  

            app.UseHttpsRedirection(); 

            app.MapControllers(); 

            app.Run(); 
        }
    }
}
