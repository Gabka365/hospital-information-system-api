using Dapper;
using HIS.Application.Database;
using HIS.Application.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
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

        public async Task<bool> DeleteRatingAsync(Guid doctorId, Guid userId, CancellationToken token = default)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var deletedRows = await connection.ExecuteAsync(new CommandDefinition("""
                delete from `HospitalInformationSystemDB`.`ratings` r
                where r.DoctorId = @doctorId and r.UserId = @userId
                """, new {doctorId, userId}, cancellationToken: token));

            return deletedRows == 1;
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

        public async Task<IEnumerable<DoctorRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var ratings = await connection.QueryAsync<DoctorRating>(new CommandDefinition("""
                select d.Id, r.Rating, d.FirstName, d.LastName, d.Surname 
                from HospitalInformationSystemDB.doctors as d
                inner join HospitalInformationSystemDB.ratings as r 
                on d.Id = r.DoctorId and r.UserId = @userId
                """, new {userId}, cancellationToken: token));

            return ratings;
        }

        public async Task<bool> RateDoctorAsync(Guid doctorId, int rating, Guid userId, CancellationToken token = default)
        {
            var connection = await _mySqlConnectionFactory.CreateConnectionAsync(token);

            var updatedRows = await connection.ExecuteAsync(new CommandDefinition("""
                insert into `HospitalInformationSystemDB`.`ratings` (UserId, DoctorId, Rating)
                values (@userId, @doctorId, @rating)
                on duplicate key update
                    UserId = @userId,
                    DoctorId = @doctorId,
                    Rating = @rating
                """, new {doctorId, rating, userId}, cancellationToken: token));

            return updatedRows == 1;
        }
    }
}
