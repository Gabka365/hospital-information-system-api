using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Models
{
    public class GetAllPatientsOptions
    {
        public Guid? UserId { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
    }
}
