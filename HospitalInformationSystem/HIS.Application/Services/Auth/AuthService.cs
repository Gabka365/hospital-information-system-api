using FluentValidation;
using FluentValidation.Results;
using HIS.Application.Mappers;
using HIS.Application.Models;
using HIS.Application.Repositories.Auth;
using HIS.Application.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HIS.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserValidator _validator;

        public AuthService(IAuthRepository authRepository, IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor, UserValidator validator) 
        { 
            _authRepository = authRepository;   
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
        }

        public async Task<User?> RegisterUserAsync(User user, CancellationToken token)
        {
            _validator.ValidateAndThrow(user);

            var userDto = user.MapToUserDto();

            var resultDto = await _authRepository.CreateUserAsync(userDto, token);

            if (resultDto == null)
            {
                throw new Exception("User with this content data cannot be registered");
            }

            var loggedUser = await LoginUserAsync(user, token);

            return loggedUser;
        }

        public async Task<User?> LoginUserAsync(User user, CancellationToken token)
        {
            //_validator.ValidateAndThrow(user);

            var userDto = await _authRepository.GetUserAsync(user.UserName, token);

            if (userDto == null)
            {
                return null;
            }

            var isValid = BCrypt.Net.BCrypt.Verify(user.Password, userDto.HashedPassword);

            if (!isValid)
            {
                return null;
            }

            var tokenSecret = _configuration["Jwt:Key"]!;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(tokenSecret);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("Id", user.Id.ToString())
            };

            var tokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(8)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwt = tokenHandler.CreateToken(tokenDecriptor);

            var jwtString = tokenHandler.WriteToken(jwt);

            _httpContextAccessor.HttpContext!.Response.Cookies.Append("jwt", jwtString);

            return user;
        }
    }
}
