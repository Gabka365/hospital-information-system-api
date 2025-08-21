using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Responses.Auth
{
    public class UserResponse
    {
        public required Guid Id { get; init; }
        public required string UserName { get; init; }
        public required string Password { get; init; }
        public required string Email { get; init; }
    }
}
