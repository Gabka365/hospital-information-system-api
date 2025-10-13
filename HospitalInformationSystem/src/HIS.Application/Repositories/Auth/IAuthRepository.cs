using HIS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<UserDTO?> CreateUserAsync (UserDTO userDto, CancellationToken token);
        Task<UserDTO?> GetUserAsync(string email, CancellationToken token);
    }
}
