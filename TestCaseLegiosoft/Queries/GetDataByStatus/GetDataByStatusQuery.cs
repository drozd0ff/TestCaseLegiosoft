using MediatR;
using System.Collections.Generic;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;

namespace TestCaseLegiosoft.Queries.GetDataByStatus
{
    public class GetDataByStatusQuery : IRequest<IEnumerable<TransactionModel>>
    {
        public TransactionStatus StatusFilter { get; }

        public GetDataByStatusQuery(TransactionStatus statusFilter)
        {
            StatusFilter = statusFilter;
        }
    }
}