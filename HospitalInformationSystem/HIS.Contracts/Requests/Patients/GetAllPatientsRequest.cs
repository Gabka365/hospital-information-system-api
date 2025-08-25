using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Requests.Patients
{
    public class GetAllPatientsRequest
    {
        public string? LastName { get; set; }
        public int? Age { get; set; }
    }
}
