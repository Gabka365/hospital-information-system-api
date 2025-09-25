using HIS.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Responses.Patients
{
    public class PatientResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public List<DiseaseList> DiseaseList { get; set; }
    }
}
