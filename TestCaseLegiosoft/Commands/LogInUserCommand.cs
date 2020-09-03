using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestCaseLegiosoft.Models;

namespace TestCaseLegiosoft.Commands
{
    public class LogInUserCommand : IRequest<Response<string>>
    {
        public UserModel User { get; }

        public LogInUserCommand(UserModel user)
        {
            User = user;
        }
    }

    public class LogInUserHandler : IRequestHandler<LogInUserCommand, Response<string>>
    {
        //TODO Hardcoded only for demo purposes, use DB instead
        private readonly List<UserModel> users = new List<UserModel>
        {
            new UserModel() {Username = "asd", Password = "123"},
            new UserModel() {Username = "Artem", Password = "123"},
            new UserModel() {Username = "Leonid", Password = "321"}
        };

        private readonly string _key;
        private readonly IConfiguration _configuration;

        public LogInUserHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = _configuration["Jwt:Key"];
        }

        public Task<Response<string>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
        {
            var authenticatedUser = AuthenticateUser(request.User);
            if (!authenticatedUser)
            {
                return Task.FromResult(Response.Fail<string>("Please log in to access data"));
            }

            var response = GenerateJsonWebToken(request.User);
            return Task.FromResult(Response.Ok<string>(response, "Copy this token to 'Authorize window'"));
        }

        private bool AuthenticateUser(UserModel user)
        {
            return users.Any(x => x.Username == user.Username && x.Password == user.Password);
        }

        private string GenerateJsonWebToken(UserModel userInfo)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userInfo.Username),
                }),
                Expires = DateTime.Now.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
