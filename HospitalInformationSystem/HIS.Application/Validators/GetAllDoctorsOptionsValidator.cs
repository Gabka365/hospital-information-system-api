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
        public GetAllDoctorsOptionsValidator()
        {
            RuleFor(x => x.Experience)
                .Must(x => x.Value > 0 && x.Value < 60)
                .WithErrorCode("406");
        }
    }
}
