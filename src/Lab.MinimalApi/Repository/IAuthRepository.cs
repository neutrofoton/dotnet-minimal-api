using Lab.MinimalApi.Dto.Login;
using Lab.MinimalApi.Dto.Login.Request;
using Lab.MinimalApi.Dto.Login.Response;

namespace Lab.MinimalApi;

public interface IAuthRepository
{
    bool IsUniqueUser(string username);
    Task<LoginResponse> Login(LoginRequest loginReques);
    Task<UserDTO> Register(RegisterationRequest registerationRequest);
}
