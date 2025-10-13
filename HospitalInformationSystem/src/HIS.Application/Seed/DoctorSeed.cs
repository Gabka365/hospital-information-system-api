using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Seed
{
    public class DoctorSeed
    {
        public Guid? Id { get; set; }
        public string UserName { get; init; }
        public string Password { get; init; }
        public string Email { get; init; }
    }
}
