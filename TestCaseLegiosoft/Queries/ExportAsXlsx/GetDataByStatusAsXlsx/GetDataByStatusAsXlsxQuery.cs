using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;

namespace TestCaseLegiosoft.Queries.ExportAsXlsx.GetDataByStatusAsXlsx
{
    public class GetDataByStatusAsXlsxQuery : IRequest<FileContentResult>
    {
        public TransactionStatus StatusFilter { get; }
        public Dictionary<PropertyInfo, bool> ModelProperties { get; set; }

        public GetDataByStatusAsXlsxQuery(TransactionStatus statusFilter, params bool[] columns)
        {
            StatusFilter = statusFilter;

            ModelProperties = new Dictionary<PropertyInfo, bool>();

            PropertyInfo[] transactionModelProperties = typeof(TransactionModel).GetProperties();

            for (int i = 0; i < columns.Length; i++)
            {
                ModelProperties.Add(transactionModelProperties[i], columns[i]);
            }
        }
    }
}