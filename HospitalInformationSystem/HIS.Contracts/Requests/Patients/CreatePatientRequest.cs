using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Requests.Patients
{
    public class CreatePatientRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string DiseaseList { get; set; }
    }
}
