using HIS.Application.DTOs;
using HIS.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Application.Mappers
{
    public static class UserDtoMapping
    {
        public static UserDTO MapToUserDto (this User user)
        {
            return new UserDTO
            {
                UserId = user.UserId,
                UserName = user.UserName,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password)
            };
        }
    }
}
