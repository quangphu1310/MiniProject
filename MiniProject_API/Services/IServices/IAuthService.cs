using MiniProject_API.Models;
using MiniProject_API.Models.DTO;

namespace MiniProject_API.Services.IServices
{
    public interface IAuthService
    {
        Task<APIResponse> Register(RegisterationRequestDTO registerationRequestDTO);
        Task<APIResponse> ConfirmEmail(string userId, string code);

    }
}
