using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Models
{
    public class User
    {
        public required Guid UserId { get; init; }
        public required string UserName { get; init; }
        public required string Password { get; init; }
    }
}
