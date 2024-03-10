using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Lab.MinimalApi.Dto.Login;
using Lab.MinimalApi.Dto.Login.Request;
using Lab.MinimalApi.Dto.Login.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Lab.MinimalApi;

public class AuthRepository : IAuthRepository
{
    private readonly ILogger<AuthRepository> logger;
    private readonly EFDbContext dbContext;
    private readonly string secretKey;

    public AuthRepository(ILogger<AuthRepository> logger, IConfiguration configuration, EFDbContext dbContext)
    {
        this.logger=logger;
        this.dbContext=dbContext;

        this.secretKey = configuration.GetValue<string>("ApiSettings:Secret")!;
    }
    public bool IsUniqueUser(string username)
    {
        return dbContext.LocalUsers.FirstOrDefault(x => x.UserName==username)==null;
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest)
    {
        var user = await dbContext.LocalUsers.FirstOrDefaultAsync(x => 
            x.UserName == loginRequest.UserName 
            && x.Password == loginRequest.Password
        );

        if(user==null){
            return new LoginResponse()
            {
                Token=null,
                User=null
            };
        }

        //build JWT token using secretKey
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user!.UserName!),
                new Claim(ClaimTypes.Role,user!.Role!),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        LoginResponse loginResponse = new()
        {
            User = user.ToUserDTO(),
            Token = new JwtSecurityTokenHandler().WriteToken(token)
        };
        return loginResponse;
    }

    public async Task<UserDTO> Register(RegisterationRequest registerationRequest)
    {
        LocalUser localUser = registerationRequest.ToLocalUser();

        await dbContext.LocalUsers.AddAsync(localUser);
        await dbContext.SaveChangesAsync();

        //make empty password before return as response
        localUser.Password=string.Empty;

        return localUser.ToUserDTO();
    }
}
