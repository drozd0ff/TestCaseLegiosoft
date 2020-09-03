using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TestCaseLegiosoft.Commands;

namespace TestCaseLegiosoft.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UploadFileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var command = new MergeWithTableCommand(file);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
