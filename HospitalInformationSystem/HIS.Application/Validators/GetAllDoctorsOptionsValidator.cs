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
            RuleFor(x => x.Experience >  0 && x.Experience < 60);
        }
    }
}
