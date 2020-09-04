using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestCaseLegiosoft.Commands.MergeWithTable;

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

        /// <summary>
        /// Task №1. 
        /// Select file "data.csv" (which is in root folder of this solution) and upload to merge transactions from
        /// this file with existed transactions. This updates only transaction status when ID matches.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var command = new MergeWithTableCommand(file);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
