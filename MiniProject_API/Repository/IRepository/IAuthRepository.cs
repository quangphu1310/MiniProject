using MiniProject_API.Models;
using MiniProject_API.Models.DTO;

namespace MiniProject_API.Repository.IRepository
{
    public interface IAuthRepository
    {
        bool IsUniqueUser(string username);
        Task<UserDTO> Register(RegisterationRequestDTO registerationRequestDTO);
        Task<bool> ConfirmEmail(string userId, string code);

    }
}
