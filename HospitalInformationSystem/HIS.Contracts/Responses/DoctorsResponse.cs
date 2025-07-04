using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Responses
{
    public class DoctorsResponse
    {
        public required IEnumerable<DoctorResponse> DoctorResponses { get; init; }
    }
}
