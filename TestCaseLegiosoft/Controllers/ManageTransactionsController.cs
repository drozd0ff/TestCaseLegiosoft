using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TestCaseLegiosoft.Commands;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;
using TestCaseLegiosoft.Queries;

namespace TestCaseLegiosoft.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ManageTransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManageTransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Task №3
        /// Get transactions filtered by status
        /// </summary>
        /// <param name="statusFilter"></param>
        /// <returns></returns>
        [HttpGet("GetDataFilteredByTransactionStatus")]
        public Task<IEnumerable<TransactionModel>> GetDataFilteredByStatus(TransactionStatus statusFilter)
        {
            var query = new GetDataByStatusQuery(statusFilter);
            var result = _mediator.Send(query);
            return result;
        }

        /// <summary>
        /// Task №3
        /// Get transactions filtered by type
        /// </summary>
        /// <param name="typeFilter"></param>
        /// <returns></returns>
        [HttpGet("GetDataFilteredByTransactionType")]
        public Task<IEnumerable<TransactionModel>> GetDataFilteredByType(TransactionType typeFilter)
        {
            var query = new GetDataByTypeQuery(typeFilter);
            var result = _mediator.Send(query);
            return result;
        }

        /// <summary>
        /// Task №4
        /// Change transaction status in transaction with given ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        [HttpPut("ChangeTransactionStatusCommand")]
        public Task<Response<TransactionModel>> ChangeTransactionStatusById(int id, TransactionStatus newStatus)
        {
            var command = new ChangeTransactionStatusCommand(id, newStatus);
            var result = _mediator.Send(command);
            return result;
        }

        /// <summary>
        /// Task №5
        /// Delete transaction with given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteTransactionByIdCommand")]
        public Task<Response<TransactionModel>> DeleteTransactionById(int id)
        {
            var command = new DeleteTransactionByIdCommand(id);
            var result = _mediator.Send(command);
            return result;
        }
    }
}
