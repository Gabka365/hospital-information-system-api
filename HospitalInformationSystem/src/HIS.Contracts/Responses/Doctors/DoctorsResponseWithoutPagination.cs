using HIS.Contracts.Responses.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Responses.Doctors
{
    public class DoctorsResponseWithoutPagination
    {
        public required IEnumerable<DoctorResponse> doctorsResponse { get; init; }

    }
}
