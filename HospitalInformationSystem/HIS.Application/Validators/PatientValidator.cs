using FluentValidation;
using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Validators
{
    public class PatientValidator : AbstractValidator<Patient>
    {
        public PatientValidator() 
        { 
            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.Age).NotEmpty().LessThan(120);
            RuleFor(x => x.DiseaseList).NotEmpty();
        }
    }
}
