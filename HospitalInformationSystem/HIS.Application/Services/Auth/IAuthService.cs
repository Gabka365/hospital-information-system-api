using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<User> RegisterUserAsync(User user, CancellationToken token);

        Task<bool> LoginUserAsync(User user, CancellationToken token);
    }
}
