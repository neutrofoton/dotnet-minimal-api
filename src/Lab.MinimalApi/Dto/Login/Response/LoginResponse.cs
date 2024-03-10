namespace Lab.MinimalApi.Dto.Login.Response;

public class LoginResponse
{
    public UserDTO? User { get; set; }
    public string? Token { get; set; }
}
