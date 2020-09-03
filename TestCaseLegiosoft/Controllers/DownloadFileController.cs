using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TestCaseLegiosoft.Models.Enums;
using TestCaseLegiosoft.Queries;

namespace TestCaseLegiosoft.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadFileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DownloadFileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get .xlsx (default Excel file format) file with ALL transactions. Click "Download file" to download file
        /// </summary>
        /// <returns>.xlsx file</returns>
        [HttpGet("AllData")]
        public async Task<IActionResult> DownloadFile()
        {
            var query = new GetAllDataAsXlsxQuery();
            var result = await _mediator.Send(query);
            return result;
        }

        /// <summary>
        /// Task №2
        /// Get .xlsx file with transactions filtered by status
        /// (default filter - one at the top of the 'select' menu, which is "Pending" atm).
        /// Choose true against each parameter if you want to include corresponding column to the result file
        /// </summary>
        /// <param name="statusFilter"></param>
        /// <param name="idColumn"></param>
        /// <param name="statusColumn"></param>
        /// <param name="typeColumn"></param>
        /// <param name="clientNameColumn"></param>
        /// <param name="amountColumn"></param>
        /// <returns></returns>
        [HttpGet("FilterByStatus")]
        public async Task<IActionResult> DownloadFileFilteredByStatus(TransactionStatus statusFilter,
            bool idColumn, bool statusColumn, bool typeColumn, bool clientNameColumn, bool amountColumn)
        {
            var query = new GetDataByStatusAsXlsxQuery(statusFilter,
                idColumn, statusColumn, typeColumn, clientNameColumn, amountColumn);
            var result = await _mediator.Send(query);
            return result;
        }

        /// <summary>
        /// Task №2
        /// Get .xlsx file with transactions filtered by type
        /// (default filter - "Refill").
        /// Choose true against each parameter if you want to include corresponding column to the result file
        /// </summary>
        /// <param name="typeFilter"></param>
        /// <param name="idColumn"></param>
        /// <param name="statusColumn"></param>
        /// <param name="typeColumn"></param>
        /// <param name="clientNameColumn"></param>
        /// <param name="amountColumn"></param>
        /// <returns></returns>
        [HttpGet("FilterByType")]
        public async Task<IActionResult> DownloadFileFilteredByType(TransactionType typeFilter,
            bool idColumn, bool statusColumn, bool typeColumn, bool clientNameColumn, bool amountColumn)
        {
            var query = new GetDataByTypeAsXlsxQuery(typeFilter,
                idColumn, statusColumn, typeColumn, clientNameColumn, amountColumn);
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
