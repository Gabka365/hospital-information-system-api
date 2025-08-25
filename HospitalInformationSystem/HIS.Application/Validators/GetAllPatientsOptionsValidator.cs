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
        public GetAllPatientsOptionsValidator() 
        {
            RuleFor(x => x.Age > 20 && x.Age < 100);
        }
    }
}
