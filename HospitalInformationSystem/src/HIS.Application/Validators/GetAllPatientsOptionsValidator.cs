using FluentValidation;
using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Validators
{
    public class GetAllPatientsOptionsValidator : AbstractValidator<GetAllPatientsOptions>
    {
        private readonly string[] AcceptedParameters = { "LastName", "Age" };

        public GetAllPatientsOptionsValidator() 
        {
            RuleFor(x => x.Age)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(130)
                .WithErrorCode("406")
                .When(x => x != null);

            RuleFor(x => x.SortField)
                .Must(x => AcceptedParameters.Contains(x))
                .When(x => x.SortField != null)
                .WithMessage(x => $"You cannot sort by this field: {x.SortField}. Only 'LastName' and 'Age'");

            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("You can't get the zero page");
        }
    }
}
