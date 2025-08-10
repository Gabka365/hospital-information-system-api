using HIS.Application.Models;
using HIS.Contracts.Requests.Auth;
using HIS.Contracts.Responses.Auth;

namespace HIS.Api.Mappers
{
    public static class UserMapping
    {
        public static User MapToRegisterUser(this UserRequest request)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Password = request.Password
            };
        }

        public static User MapToLoggedUser(this UserRequest request)
        {
            return new User
            {
                UserName = request.UserName,
                Password = request.Password,
                Trusted = request.Trusted
            };
        }

        public static UserResponse MapToResponse(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Password = user.Password
            };
        }
    }
}
