using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        //[HttpGet]
        //public async Task<IActionResult> GetAllTransactions()
        //{
        //    var query = new GetAllTransactionsQuery();
        //    var result = await _mediator.Send(query);
        //    return Ok(result);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetTransactionById(int id)
        //{
        //    var query = new GetTransactionById(id);
        //    var result = await _mediator.Send(query);
        //    return result != null ? (IActionResult) Ok(result) : NotFound();
        //}

        [HttpGet("AllData")]
        public async Task<IActionResult> DownloadFile()
        {
            var query = new GetAllDataAsXlsxQuery();
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpGet("FilterByStatus")]
        public async Task<IActionResult> DownloadFileFiltratedByStatus(TransactionStatus statusFilter,
            bool idColumn, bool statusColumn, bool typeColumn, bool clientNameColumn, bool amountColumn)
        {
            var query = new FilterByStatusQuery(statusFilter,
                idColumn, statusColumn, typeColumn, clientNameColumn, amountColumn);
            var result = await _mediator.Send(query);
            return result;
        }

        [HttpGet("FilterByType")]
        public async Task<IActionResult> DownloadFileFiltratedByType(TransactionType typeFilter,
            bool idColumn, bool statusColumn, bool typeColumn, bool clientNameColumn, bool amountColumn)
        {
            var query = new FilterByTypeQuery(typeFilter,
                idColumn, statusColumn, typeColumn, clientNameColumn, amountColumn);
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
