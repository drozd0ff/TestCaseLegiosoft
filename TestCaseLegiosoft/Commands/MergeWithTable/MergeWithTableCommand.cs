using MediatR;
using Microsoft.AspNetCore.Http;

namespace TestCaseLegiosoft.Commands.MergeWithTable
{
    public class MergeWithTableCommand : IRequest<string>
    {
        public IFormFile File { get; }

        public MergeWithTableCommand(IFormFile file)
        {
            File = file;
        }
    }
}