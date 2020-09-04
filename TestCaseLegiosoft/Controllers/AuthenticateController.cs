using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestCaseLegiosoft.Commands;
using TestCaseLegiosoft.Commands.LogInUser;
using TestCaseLegiosoft.Models;

namespace TestCaseLegiosoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticateController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Enter valid username and password to generate JSON Web Token,
        /// which you should copy-paste in "Authorize" window at the top right corner of the Swagger UI
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("LogIn")]
        public async Task<Response<string>> LogIn([FromBody] UserModel user)
        {
            var command = new LogInUserCommand(user);
            return await _mediator.Send(command);
        }

        /// <summary>
        /// Get your JSON Web Token via HttpContext.GetTokenAsync("access_token")
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetToken")]
        public async Task<IEnumerable<string>> GetToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            return new string[] { accessToken };
        }
    }
}
