using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Queries.GetDataByType
{
    public class GetDataByTypeHandler : IRequestHandler<GetDataByTypeQuery, IEnumerable<TransactionModel>>
    {
        private readonly DataContext _dataContext;

        public GetDataByTypeHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IEnumerable<TransactionModel>> Handle(GetDataByTypeQuery request, CancellationToken cancellationToken)
        {
            return await _dataContext.TransactionModels
                .Where(x => x.TransactionType == request.TypeFilter).ToListAsync();
        }
    }
}
