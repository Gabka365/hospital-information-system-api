using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Contracts.Responses
{
    public class ValidationErrorResponse
    {
        public required IEnumerable<ValidationError> Errors { get; init; }
    }

    public class ValidationError
    {
        public required string PropertyName { get; init; }
        public required string ErrorMessage { get; init; }
    }
}
