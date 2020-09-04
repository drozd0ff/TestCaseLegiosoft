using MediatR;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;

namespace TestCaseLegiosoft.Commands.ChangeTransactionStatus
{
    public class ChangeTransactionStatusCommand : IRequest<Response<TransactionModel>>
    {
        public int Id { get; set; }
        public TransactionStatus NewStatus { get; set; }

        public ChangeTransactionStatusCommand(int id, TransactionStatus newStatus)
        {
            Id = id;
            NewStatus = newStatus;
        }
    }
}