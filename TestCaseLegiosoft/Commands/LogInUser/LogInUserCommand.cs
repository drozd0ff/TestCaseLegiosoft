using MediatR;
using TestCaseLegiosoft.Models;

namespace TestCaseLegiosoft.Commands.LogInUser
{
    public class LogInUserCommand : IRequest<Response<string>>
    {
        public UserModel User { get; }

        public LogInUserCommand(UserModel user)
        {
            User = user;
        }
    }
}