using FluentValidation;
using FluentValidation.Results;
using HIS.Application.Models;
using HIS.Application.Repositories.Ratings;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Services.Ratings
{
    public class RatingsService : IRatingsService
    {
        private readonly IRatingsRepository _ratingsRepository;

        public RatingsService(IRatingsRepository ratingsRepository)
        {
            _ratingsRepository = ratingsRepository;
        }

        public async Task<bool> DeleteRatingAsync(Guid doctorId, Guid userId, CancellationToken token = default)
        {
            return await _ratingsRepository.DeleteRatingAsync(doctorId, userId, token);
        }

        public async Task<IEnumerable<DoctorRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
        {
            var ratings = await _ratingsRepository.GetRatingsForUserAsync(userId, token);

            return ratings;
        }

        public async Task<bool> RateDoctorAsync(Guid doctorId, int rating, Guid userId, CancellationToken token = default)
        {
            if (rating > 10 || rating < 0) 
            {
                throw new ValidationException("Error with validation has been occured.", new[]
                {
                    new ValidationFailure
                    {
                        PropertyName = nameof(rating),
                        ErrorMessage = $"Incorrect rating number. Number can be only less than 10 and more than 0."
                    }
                });
            }

            var isRated = await _ratingsRepository.RateDoctorAsync(doctorId, rating, userId, token);
            
            return isRated;
        }
    }
}
