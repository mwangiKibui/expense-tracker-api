using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTracker.Helpers
{   
    public class AuthenticatedUser {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public required String FirstName { get; set; }
        public required String LastName { get; set; }
        public required String Email { get; set; }
    }
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

        public static async Task<AuthenticatedUser?> GetAuthenticatedUser (Data.DataContext dbContext,ClaimsPrincipal user){
            string? authenticatedUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            AuthenticatedUser? authenticatedUser = await dbContext.Users.Where(usr => usr.UserID.ToString() == authenticatedUserId).Select(
                user => new AuthenticatedUser {
                    Id = user.Id,
                    Email = user.Email,
                    UserId = user.UserID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                }
            ).FirstOrDefaultAsync();
            return authenticatedUser;
        }
    }
}