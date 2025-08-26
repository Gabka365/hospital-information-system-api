using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace HIS.Application.Models
{
    public class GetAllDoctorsOptions
    {
        public Guid? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Surname { get; set; }
        public int? Experience { get; set; }
        public string? Specialties { get; set; }
        public string? Category { get; set; }
        public string? SortField { get; set; }
        public SortOrder? SortOrder {  get; set; }
    }

    public enum SortOrder
    { 
        Unsorted,
        Ascending,
        Descending
    }
}
