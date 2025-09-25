using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Requests.Patients
{
    public class CreatePatientRequest
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Surname { get; init; }
        public required int Age { get; init; }
        public required string DiseaseList { get; init; }
        public required string Email { get; init; }
    }
}
