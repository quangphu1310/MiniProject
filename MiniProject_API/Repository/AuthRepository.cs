using Microsoft.AspNetCore.Identity;
using MiniProject_API.Data;
using MiniProject_API.Models;
using MiniProject_API.Models.DTO;
using MiniProject_API.Repository.IRepository;
using MiniProject_API.Services.IServices;
using System.ComponentModel.DataAnnotations;

namespace MiniProject_API.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AuthRepository(ApplicationDbContext db,
                                UserManager<ApplicationUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                IEmailService emailService)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        public bool IsUniqueUser(string username)
        {
            if (_db.ApplicationUsers.FirstOrDefault(x => x.UserName == username) != null)
                return false;
            return true;
        }

        public async Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            if (!new EmailAddressAttribute().IsValid(registerationRequestDTO.UserName))
            {
                throw new Exception("The specified string is not in the form required for an e-mail address.");
            }

            ApplicationUser user = new()
            {
                UserName = registerationRequestDTO.UserName,
                Name = registerationRequestDTO.Name,
                Email = registerationRequestDTO.UserName,
                NormalizedEmail = registerationRequestDTO.UserName.ToUpper()
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerationRequestDTO.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("user"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("user"));
                    }
                    await _userManager.AddToRoleAsync(user, "user");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //await _emailService.SendEmail(user.Email, "Email Confirmation", code);
                    var confirmationLink = $"https://localhost:7068/api/auth/confirm-email?userId={user.Id}&code={Uri.EscapeDataString(code)}";
                    await _emailService.SendEmail(user.Email, "Email Confirmation", confirmationLink);

                    return new UserDTO
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                    };
                }
                else
                {
                    string errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception(errors);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result.Succeeded;
        }
    }
}
