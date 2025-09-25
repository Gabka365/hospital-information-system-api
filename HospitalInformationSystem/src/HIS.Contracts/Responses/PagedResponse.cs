using HIS.Contracts.Responses.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIS.Contracts.Responses
{
    public class PagedResponse<TResponse>
    {
        public required IEnumerable<TResponse> Items { get; init; } = Enumerable.Empty<TResponse>();
        public required int PageSize { get; init; }
        public required int Page { get; init; }
        public required int Total { get; init; }

        public bool HasNextPage => Total > (Page * PageSize);

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<Link> Links { get; set; } = new();
    }

    public class Link
    { 
        public required string Href {  get; init; }
        public required string Rel { get; init; }
        public required string Type { get; init; }
        public required string Name { get; init; }
    }
}
