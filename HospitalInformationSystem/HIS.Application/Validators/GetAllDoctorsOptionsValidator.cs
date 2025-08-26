using FluentValidation;
using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Validators
{
    public class GetAllDoctorsOptionsValidator : AbstractValidator<GetAllDoctorsOptions>
    {
        private readonly string[] AcceptedParameters = { "LastName", "Experience" };

        public GetAllDoctorsOptionsValidator()
        {
            RuleFor(x => x.Experience)
                .Must(x => x.Value > 0 && x.Value < 60)
                .When(x => x.Experience is not null)
                .WithErrorCode("406");

            RuleFor(x => x.SortField)
                .Must(x => AcceptedParameters.Contains(x))
                .When(x => x.SortField is not null)
                .WithMessage(x => $"You cannot sort by this field: {x.SortField}. Only 'LastName' and 'Experience'");
        }
    }
}
