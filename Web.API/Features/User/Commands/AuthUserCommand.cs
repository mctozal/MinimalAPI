using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using API.Persistence;
using MediatR;
using Microsoft.IdentityModel.Tokens;

public class AuthUserCommand : IRequest<string>
{
    public string? UserName { get; set; }
    public string? Password { get; set; }

    public AuthUserCommand(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}

public class AuthUserCommandHandler(AppDbContext dbContext) : IRequestHandler<AuthUserCommand, string>
{
    
    public async Task<string> Handle(AuthUserCommand request, CancellationToken cancellationToken)
    {
        // implement userservicelogic
        // if(!result){
        //     return "Invalid username of password";
        // }

        string token = GenerateJwtToken(request.UserName!);
        
        return token;
    }
    
    private string GenerateJwtToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Name,"Security")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret_secret_secret_secret_secret_secret_secret"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);
    
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

        return encodedJwt;
    }
}