﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Commands
{
    public class DeleteTransactionByIdCommand : IRequest<Response<TransactionModel>>
    {
        public int Id { get; set; }
        public DeleteTransactionByIdCommand(int id)
        {
            Id = id;
        }
    }

    public class DeleteTransactionByIdHandler : IRequestHandler<DeleteTransactionByIdCommand, Response<TransactionModel>>
    {
        private readonly DataContext _dataContext;

        public DeleteTransactionByIdHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Response<TransactionModel>> Handle(DeleteTransactionByIdCommand request, CancellationToken cancellationToken)
        {
            var transactionWithGivenId = _dataContext.TransactionModels
                .Where(x => x.TransactionId == request.Id).FirstOrDefault();

            if (transactionWithGivenId == null)
            {
                return Response.Fail<TransactionModel>("The transaction with given key was not found");
            }

            _dataContext.TransactionModels.Remove(transactionWithGivenId);

            await _dataContext.SaveChangesAsync();

            return Response.Ok<TransactionModel>(transactionWithGivenId, "Transaction has been deleted succesfully");
        }
    }
}
