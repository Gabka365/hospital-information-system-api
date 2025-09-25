using HIS.Contracts.Responses.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Responses.Patients
{
    public class PatientsResponseWithoutPagination
    {
        public required IEnumerable<PatientResponse> patientsResponse { get; init; }

    }
}
