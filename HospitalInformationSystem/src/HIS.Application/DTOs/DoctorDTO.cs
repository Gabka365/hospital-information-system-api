using HIS.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.DTOs
{
    public class DoctorDTO
    {
        public Guid Id { get; init; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string Specialties { get; init; }
        public string Category { get; init; }
        public int Experience { get; set; }
        public float? Rating { get; set; }
        public int UserRating { get; set; }
    }
}
