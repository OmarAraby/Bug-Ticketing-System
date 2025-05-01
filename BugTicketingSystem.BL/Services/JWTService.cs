using BugTicketingSystem.BL.Dtos.UserDtos;
using BugTicketingSystem.DL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.BL.Services
{
    public class JWTService
    {
        private readonly string _secretKey;
        private readonly int _tokenExpiryHours;

        public JWTService(IConfiguration configuration)
        {
            _secretKey = configuration["JWT:SecretKey"]
                ?? throw new ArgumentException("JWT Secret Key is missing in configuration");


            
            _tokenExpiryHours = int.TryParse(configuration["JWT:TokenExpiryHours"], out var expiryHours) ? expiryHours : 1;
        }

        public AuthResponseDto GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim("userName", user.UserName),
                new Claim("email", user.Email)
            };

            if (user.UserRoles?.Any() == true)
            {
                foreach (var role in user.UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Role.ToString()));
                }
            }

         
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_tokenExpiryHours),
                SigningCredentials = credentials
            };

    
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

    
            return new AuthResponseDto
            {
                UserId = user.UserId, // Set the UserId  just for test when i add bug i will need it 
                UserName = user.UserName,
                Email = user.Email,
                Token = tokenString,
                ExpireDate = token.ValidTo,
            };
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }
    }
}
