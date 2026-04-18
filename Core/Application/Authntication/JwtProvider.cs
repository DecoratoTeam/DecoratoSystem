using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authntication
{
    public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IjwtProvider
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public (string Token, int ExpiersIn) GenerateJwtToken(User user)
        {
            Claim[] claims = [
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, ((int)user.Role).ToString())
            ];

            var symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtOptions.Key));

            var signingCredentials = new SigningCredentials(
                symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryMinutes)
                );

            return (new JwtSecurityTokenHandler().WriteToken(token), 
                    _jwtOptions.ExpiryMinutes * 60);
        }
    }
}

