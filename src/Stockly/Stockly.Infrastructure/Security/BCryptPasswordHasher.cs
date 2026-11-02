using Stockly.Application.Common.Interfaces;

namespace Stockly.Infrastructure.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
            => BCrypt.Net.BCrypt.HashPassword(password);

        public bool VerifyPassword(string password, string passwordHash)
            => BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
