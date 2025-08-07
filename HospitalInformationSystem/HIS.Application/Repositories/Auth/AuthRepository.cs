using HIS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        public Task<bool> CreateUserAsync(UserDTO userDto, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
