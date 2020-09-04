using MediatR;
using System.Collections.Generic;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;

namespace TestCaseLegiosoft.Queries.GetDataByType
{
    public class GetDataByTypeQuery : IRequest<IEnumerable<TransactionModel>>
    {
        public TransactionType TypeFilter { get; }

        public GetDataByTypeQuery(TransactionType typeFilter)
        {
            TypeFilter = typeFilter;
        }
    }
}