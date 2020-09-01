using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TestCaseLegiosoft.Commands;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly IMediator _mediator;
        private readonly ILogger<UploadFileController> _logger;


        public UploadFileController(IMediator mediator, ILogger<UploadFileController> logger, DataContext context, IConfiguration configuration)
        {
            Configuration = configuration;
            _mediator = mediator;
            _logger = logger;
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
