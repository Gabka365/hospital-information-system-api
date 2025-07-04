using HIS.Contracts.Enums;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Requests
{
    public class CreateDoctorRequest
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Surname { get; init; }
        public required List<Speciality> Specialties { get; init; }
        public required Category Category { get; init; }
        public required int Experience { get; init; }
    }
}
