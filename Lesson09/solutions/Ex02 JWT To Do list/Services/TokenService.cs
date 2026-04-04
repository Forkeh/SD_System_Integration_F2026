using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ex_07_To_Do_List.Data;
using Microsoft.IdentityModel.Tokens;

namespace Ex_07_To_Do_List.Services;

public class TokenService(IConfiguration config)
{
    private readonly string _issuer = config["Jwt:Issuer"]!;
    private readonly string _secret = config["Jwt:Secret"]!;

    public int ExpiryMinutes { get; } = int.Parse(config["Jwt:ExpiryMinutes"]!);

    public string CreateToken(User user, out string jti)
    {
        jti = Guid.NewGuid().ToString(); // unique token ID

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jti),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var token = new JwtSecurityToken(
            _issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(ExpiryMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}