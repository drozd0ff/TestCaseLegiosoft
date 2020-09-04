using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;

namespace TestCaseLegiosoft.Queries.ExportAsXlsx.GetDataByTypeAsXlsx
{
    public class GetDataByTypeAsXlsxQuery : IRequest<FileContentResult>
    {
        public TransactionType TypeFilter { get; set; }
        public Dictionary<PropertyInfo, bool> ModelProperties { get; set; }

        public GetDataByTypeAsXlsxQuery(TransactionType type, params bool[] columns)
        {
            TypeFilter = type;

            ModelProperties = new Dictionary<PropertyInfo, bool>();

            PropertyInfo[] transactionModelProperties = typeof(TransactionModel).GetProperties();

            for (int i = 0; i < columns.Length; i++)
            {
                ModelProperties.Add(transactionModelProperties[i], columns[i]);
            }
        }
    }
}