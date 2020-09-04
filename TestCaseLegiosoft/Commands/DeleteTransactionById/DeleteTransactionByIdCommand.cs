using MediatR;
using TestCaseLegiosoft.Models;

namespace TestCaseLegiosoft.Commands.DeleteTransactionById
{
    public class DeleteTransactionByIdCommand : IRequest<Response<TransactionModel>>
    {
        public int Id { get; set; }
        public DeleteTransactionByIdCommand(int id)
        {
            Id = id;
        }
    }
}