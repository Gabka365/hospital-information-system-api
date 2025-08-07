using HIS.Application.Mappers;
using HIS.Application.Models;
using HIS.Application.Repositories.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository) 
        { 
            _authRepository = authRepository;   
        }

        public async Task<User> RegisterUserAsync(User user, CancellationToken token)
        {
            var userDto = user.MapToUserDto();

            var resultDto = await _authRepository.CreateUserAsync(userDto, token);

            throw new NotImplementedException();
        }

        public Task<User> LoginUserAsync(User user, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
