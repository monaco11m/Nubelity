
using Nubelity.Domain.Entities;

namespace Nubelity.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }
}
