using HIS.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.DTOs
{
    public class DoctorDto
    {
        public required Guid Id { get; init; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Surname { get; set; }
        public required string Specialties { get; init; }
        public required string Category { get; init; }
        public required int Experience { get; set; }
    }
}
