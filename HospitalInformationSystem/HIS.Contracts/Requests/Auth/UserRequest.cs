using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Requests.Auth
{
    public class UserRequest
    {
        public required string UserName { get; init; }
        public required string Password { get; init; }
    }
}
