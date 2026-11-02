using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Stockly.Domain.Entities;
using Stockly.Application.Common.DTOs;
using Stockly.Application.Common.Interfaces;
using System.Security.Claims;
using System.Text;

namespace Stockly.Infrastructure.Security
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenResult GenerateToken(User user)
        {
            var accessExpiry = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"] ?? "15"));
            var RefreshExpiry = DateTime.UtcNow.AddDays(double.Parse(_configuration["JwtSettings:RefreshTokenExpirationDays"] ?? "7"));
            var accessToken = GenerateAccessToken(user, accessExpiry, _configuration.GetSection("JwtSettings"));
            var refreshToken = GenerateRefreshToken();

            return new TokenResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = accessExpiry,
                RefreshTokenExpiration = RefreshExpiry
            };
        }

        private string GenerateAccessToken(User user, DateTime accessExpiry, IConfigurationSection jwtSettings)
        {
            var secrectKey = jwtSettings["SecretKey"] ?? throw new Exception("JWT Secret Key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrectKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new Dictionary<string, object>
            {
                [ClaimTypes.NameIdentifier] = user.Id.ToString(),
                [ClaimTypes.Name] = user.FullName,
                [ClaimTypes.Email] = user.Email,
                [ClaimTypes.Role] = user.Role.ToString(),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                Claims = claims,
                Expires = accessExpiry,
                SigningCredentials = creds,
            };

            var tokenhandler = new JsonWebTokenHandler();
            var token = tokenhandler.CreateToken(tokenDescriptor);

            return token;
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
