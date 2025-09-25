using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public required string UserName { get; init; }
        public required string HashedPassword { get; init; }
        public required string Email { get; init; }
    }
}
