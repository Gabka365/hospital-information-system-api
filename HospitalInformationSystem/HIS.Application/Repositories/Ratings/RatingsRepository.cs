using Dapper;
using HIS.Application.Database;
using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Repositories.Ratings
{
    public class RatingsRepository : IRatingsRepository
    {
        private readonly MySqlConnectionFactory _mySqlConnectionFactory;

        public RatingsRepository(MySqlConnectionFactory mySqlConnectionFactory)
        {
            _mySqlConnectionFactory = mySqlConnectionFactory;
        }


        public Task<bool> DeleteRatingAsync(Guid doctorId, Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<float?> GetRatingAsync(Guid doctorId, CancellationToken token = default)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.QuerySingleOrDefaultAsync<float?>(new CommandDefinition("""
                select round(avg(r.Rating), 1) as Rating
                from `HospitalInformationSystemDB`.`ratings` r
                where r.DoctorId = @doctorId
                group by r.DoctorId
                """, new { doctorId }, cancellationToken: token));

            return result.Value;
        }

        public async Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid doctorId, Guid userId, CancellationToken token = default)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var result = await connection.QuerySingleOrDefaultAsync<(float?, int?)>(new CommandDefinition("""
                select round(avg(r.Rating), 1) as Rating, (
                	select myr.Rating 
                    from `HospitalInformationSystemDB`.`ratings` myr 
                    where myr.UserId = @userId 
                    and myr.DoctorId = @doctorId
                    ) as UserRating
                from `HospitalInformationSystemDB`.`ratings` r
                where r.DoctorId = @doctorId
                group by r.DoctorId
                """, new { doctorId, userId }, cancellationToken: token));

            return (result.Item1, result.Item2);
        }

        public Task<IEnumerable<DoctorRating>> GetRatingsForUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RateDoctorAsync(Guid doctorId, int rating, Guid userId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
