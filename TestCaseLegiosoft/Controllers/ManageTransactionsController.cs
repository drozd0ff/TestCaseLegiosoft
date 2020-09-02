using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestCaseLegiosoft.Commands;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;
using TestCaseLegiosoft.Queries;

namespace TestCaseLegiosoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageTransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManageTransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetDataFilteredByTransactionStatus")]
        public Task<IEnumerable<TransactionModel>> GetDataFilteredByStatus(TransactionStatus statusFilter)
        {
            var query = new GetDataByStatusQuery(statusFilter);
            var result = _mediator.Send(query);
            return result;
        }

        [HttpGet("GetDataFilteredByTransactionType")]
        public Task<IEnumerable<TransactionModel>> GetDataFilteredByType(TransactionType typeFilter)
        {
            var query = new GetDataByTypeQuery(typeFilter);
            var result = _mediator.Send(query);
            return result;
        }

        [HttpPut("ChangeTransactionStatusCommand")]
        public Task<Response<TransactionModel>> ChangeTransactionStatusById(int id, TransactionStatus newStatus)
        {
            var command = new ChangeTransactionStatusCommand(id, newStatus);
            var result = _mediator.Send(command);
            return result;
        }

        [HttpDelete("DeleteTransactionByIdCommand")]
        public Task<Response<TransactionModel>> DeleteTransactionById(int id)
        {
            var command = new DeleteTransactionByIdCommand(id);
            var result = _mediator.Send(command);
            return result;
        }
    }
}
