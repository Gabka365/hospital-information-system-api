using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Requests.Doctors
{
    public class GetAllDoctorsRequest
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Surname { get; init; }
        public int? Experience { get; init; }
        public string? Specialties { get; init; }
        public string? Category { get; init; }
        public string? SortBy { get; init; }
    }
}
