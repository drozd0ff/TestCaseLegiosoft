using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Commands.ChangeTransactionStatus
{
    public class ChangeTransactionStatusHandler : IRequestHandler<ChangeTransactionStatusCommand, Response<TransactionModel>>
    {
        private readonly DataContext _dataContext;

        public ChangeTransactionStatusHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Response<TransactionModel>> Handle(ChangeTransactionStatusCommand request, CancellationToken cancellationToken)
        {
            var transactionWithGivenId = _dataContext.TransactionModels
                .FirstOrDefault(x => x.TransactionId == request.Id);

            if (transactionWithGivenId == null)
            {
                return Response.Fail<TransactionModel>("The transaction with given key was not found");
            }
            if (transactionWithGivenId.TransactionStatus == request.NewStatus)
            {
                return Response.Fail<TransactionModel>("The transaction is already in this status");
            }

            var updatedTransaction = transactionWithGivenId;
            updatedTransaction.TransactionStatus = request.NewStatus;

            // Use this approach instead of 'Update' because 'Update' throws InvalidOperationException
            // "The instance of entity type ... cannot be tracked
            // because another instance of this type with the same key is already being tracked"
            // (or you can override .Update())
            _dataContext.Entry(transactionWithGivenId).CurrentValues.SetValues(updatedTransaction);

            await _dataContext.SaveChangesAsync();

            return Response.Ok<TransactionModel>(updatedTransaction, "Transaction status is updated!");
        }
    }
}
