using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public required string UserName { get; init; }
        public required string Password { get; init; }
        public required string Email { get; init; }
    }
}
