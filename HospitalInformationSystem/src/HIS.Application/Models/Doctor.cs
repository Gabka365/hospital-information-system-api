using HIS.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Models
{
    public class Doctor
    {
        public required Guid Id { get; init; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Surname { get; set; }
        public float? Rating { get; set; }
        public int? UserRating { get; set; }
        public required List<Speciality> Specialties { get; init; }
        public required Category Category { get; init; }
        public required int Experience { get; set; }
    }
}
