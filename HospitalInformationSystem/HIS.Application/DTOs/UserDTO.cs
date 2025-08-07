using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.DTOs
{
    public class UserDTO
    {
        public required Guid UserId { get; init; }
        public required string UserName { get; init; }
        public required string HashedPassword { get; init; }
    }
}
