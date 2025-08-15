using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Models
{
    public class DoctorRating
    {
        public required Guid DoctorId { get; init; }
        public required int Rating {  get; init; }
    }
}
