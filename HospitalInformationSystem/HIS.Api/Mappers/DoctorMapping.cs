using HIS.Application.Models;
using HIS.Contracts.Requests;
using HIS.Contracts.Requests.Doctors;
using HIS.Contracts.Responses;
using HIS.Contracts.Responses.Doctors;
using System.Runtime.CompilerServices;

namespace HIS.Api.Mappers
{
    public static class DoctorMapping
    {
        public static Doctor MapToDoctor(this CreateDoctorRequest request, Guid specifiedUserId)
        {
            return new Doctor
            {
                Id = specifiedUserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Surname = request.Surname,
                Specialties = request.Specialties,
                Category = request.Category,
                Experience = request.Experience
            }; 
        }

        public static Doctor MapToDoctor(this UpdateDoctorRequest request, Guid id)
        {
            return new Doctor
            {
                Id = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Surname = request.Surname,
                Specialties = request.Specialties,
                Category = request.Category,
                Experience = request.Experience
            };
        }


        public static DoctorResponse MapToResponse(this Doctor doctor)
        {
            return new DoctorResponse
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Surname= doctor.Surname,    
                Specialties= doctor.Specialties,
                Category = doctor.Category,
                Experience = doctor.Experience,
                Rating = doctor.Rating,
                UserRating = doctor.UserRating
            };
        }

        public static DoctorsResponse MapToResponses(this List<Doctor> doctors)
        {
            return new DoctorsResponse { DoctorResponses = doctors.Select(x => x.MapToResponse()).ToList()};
        }
    }
}
