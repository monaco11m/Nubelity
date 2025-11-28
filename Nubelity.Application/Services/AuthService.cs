

using Nubelity.Application.Exceptions;
using Nubelity.Application.Interfaces;

namespace Nubelity.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _unitOfWork.UserRepository.GetByUsernameAsync(username)
                ?? throw new UnauthorizedException("Invalid credentials");

            if (user.PasswordHash != password)
                throw new UnauthorizedException("Invalid credentials");

            return _jwtService.GenerateJwtToken(user);
        }
    }

}
