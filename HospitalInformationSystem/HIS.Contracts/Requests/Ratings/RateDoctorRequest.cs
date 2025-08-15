using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Requests.Ratings
{
    public class RateDoctorRequest
    {
        public required int Rating { get; init; }
    }
}
