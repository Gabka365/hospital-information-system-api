using HIS.Application.Models;
using HIS.Contracts.Requests.Auth;

namespace HIS.Api.Mappers
{
    public static class UserMapping
    {
        public static User MapToUser(this UserRequest request)
        {
            return new User
            {
                UserId = Guid.NewGuid(),
                UserName = request.UserName,
                Password = request.Password
            };
        }
    }
}
