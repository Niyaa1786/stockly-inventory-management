using Stockly.Domain.Entities;
using Stockly.Application.Common.DTOs;

namespace Stockly.Application.Common.Interfaces;

public interface ITokenGenerator
{
    TokenResult GenerateToken(User user);
}
