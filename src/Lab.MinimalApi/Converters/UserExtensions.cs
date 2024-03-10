using Lab.MinimalApi.Dto.Login;
using Lab.MinimalApi.Dto.Login.Request;

namespace Lab.MinimalApi;

public static class UserExtensions
{
    public static LocalUser ToLocalUser(this RegisterationRequest registerationRequest)
    {
        return new LocalUser()
        {
            Name = registerationRequest.Name,
            UserName = registerationRequest.UserName,
            Password = registerationRequest.Password,
            Role = "admin" //for example only
        };
    }

    public static UserDTO ToUserDTO(this LocalUser localUser)
    {
        return new UserDTO()
        {
            Name = localUser.Name,
            UserName = localUser.UserName
        };
    }
}
