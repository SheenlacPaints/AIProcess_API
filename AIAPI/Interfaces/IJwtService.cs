using AIAPI.DTOs;

namespace AIAPI.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(string userName, out DateTime expiry);
    }
}
