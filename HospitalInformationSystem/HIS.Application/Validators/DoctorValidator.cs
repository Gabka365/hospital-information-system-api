using FluentValidation;
using HIS.Application.Models;
using HIS.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Validators
{
    public class DoctorValidator : AbstractValidator<Doctor>
    {
        public DoctorValidator() 
        {
            RuleFor(x => x.Id)
                .Must(x => x.ToString().Length == 36)
                .WithErrorCode("406");
            
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithErrorCode("406");
            
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithErrorCode("406");
            
            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithErrorCode("406");

            RuleFor(x => x.Specialties)
                .NotEmpty()
                .Must(x => x.Count <= 10)
                .WithErrorCode("406");
                
            RuleFor(x => x.Category)
                .NotEmpty()
                .WithErrorCode("406");
            
            RuleFor(x => x.Experience)
                .NotEmpty()
                .Must(x => x < 80)
                .WithErrorCode("406");
        }
    }
}
