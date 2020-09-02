using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Queries
{
    public class GetDataByStatusQuery : IRequest<IEnumerable<TransactionModel>>
    {
        public TransactionStatus StatusFilter { get; }

        public GetDataByStatusQuery(TransactionStatus statusFilter)
        {
            StatusFilter = statusFilter;
        }
    }

    public class GetDataByStatusHandler : IRequestHandler<GetDataByStatusQuery, IEnumerable<TransactionModel>>
    {
        private readonly DataContext _dataContext;

        public GetDataByStatusHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IEnumerable<TransactionModel>> Handle(GetDataByStatusQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.TransactionModels
                .Where(x => x.TransactionStatus == request.StatusFilter).ToListAsync();
        }
    }
}
