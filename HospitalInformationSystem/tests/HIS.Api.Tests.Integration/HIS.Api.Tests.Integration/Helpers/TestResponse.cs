using HIS.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Api.Tests.Integration.Helpers
{
    public class TestResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public List<Speciality> Specialties { get; set; }
        public Category Category { get; set; }
        public int Experience { get; set; }
        public float? Rating { get; set; }
        public int? UserRating { get; set; }
    }
}
