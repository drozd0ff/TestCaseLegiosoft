using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Queries.GetDataByStatus
{
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
