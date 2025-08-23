using HIS.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Responses.Doctors
{
    public class DoctorResponse
    {
        public required Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Surname { get; init; }
        public required List<Speciality> Specialties { get; init; }
        public required Category Category { get; init; }
        public required int Experience { get; init; }
        public float? Rating { get; init; }
        public int? UserRating { get; init; }
    }
}
