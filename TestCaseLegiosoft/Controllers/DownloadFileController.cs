using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestCaseLegiosoft.Models.Enums;
using TestCaseLegiosoft.Queries;

namespace TestCaseLegiosoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadFileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DownloadFileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("AllData")]
        public async Task<IActionResult> DownloadFile()
        {
            var query = new GetAllDataAsXlsxQuery();
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpGet("FilterByStatus")]
        public async Task<IActionResult> DownloadFileFilteredByStatus(TransactionStatus statusFilter,
            bool idColumn, bool statusColumn, bool typeColumn, bool clientNameColumn, bool amountColumn)
        {
            var query = new FilterByStatusQuery(statusFilter,
                idColumn, statusColumn, typeColumn, clientNameColumn, amountColumn);
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpGet("FilterByType")]
        public async Task<IActionResult> DownloadFileFilteredByType(TransactionType typeFilter,
            bool idColumn, bool statusColumn, bool typeColumn, bool clientNameColumn, bool amountColumn)
        {
            var query = new FilterByTypeQuery(typeFilter,
                idColumn, statusColumn, typeColumn, clientNameColumn, amountColumn);
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
