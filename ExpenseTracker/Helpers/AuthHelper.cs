using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseTracker.Models;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTracker.Helpers
{
    public class AuthHelper
    {
        public string GenerateToken(User user,String privateKey,int tokenLifetime ){
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(privateKey);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
            );
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddMinutes(tokenLifetime),
                SigningCredentials = credentials
            };
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private static ClaimsIdentity GenerateClaims(User user){
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier,user.UserID.ToString()));
            return claims;
        }
    }
}