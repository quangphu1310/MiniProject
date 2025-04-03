using MiniProject_API.Models;
using MiniProject_API.Models.DTO;
using MiniProject_API.Repository.IRepository;
using MiniProject_API.Services.IServices;
using System.Net;

namespace MiniProject_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly APIResponse _apiResponse;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
            _apiResponse = new APIResponse();
        }

        public async Task<APIResponse> ConfirmEmail(string userId, string code)
        {
            try
            {
                bool result = await _authRepository.ConfirmEmail(userId, code);
                if (!result)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Errors.Add("Invalid confirmation request");
                    return _apiResponse;
                }

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Result = "Email confirmed successfully!";
                return _apiResponse;
            }
            catch (Exception ex) {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
            
        }

        public async Task<APIResponse> Register(RegisterationRequestDTO requestDTO)
        {
            bool isUnique = _authRepository.IsUniqueUser(requestDTO.UserName);
            if (!isUnique)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.Errors.Add("User already exists");
                return _apiResponse;
            }
            try
            {
                var user = await _authRepository.Register(requestDTO);
                if (user == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Errors.Add("Error while registeration!");
                    return _apiResponse;
                }
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = user;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
    }
}
