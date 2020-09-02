using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TestCaseLegiosoft.Extensions;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Queries
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

    public class GetDataByTypeAsXlsxHandler : IRequestHandler<GetDataByTypeAsXlsxQuery, FileContentResult>
    {
        private readonly DataContext _dataContext;

        public GetDataByTypeAsXlsxHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<FileContentResult> Handle(GetDataByTypeAsXlsxQuery request, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[16 * 1024];
            using (var workbook = new XLWorkbook())
            {
                workbook.InsertData(_dataContext, request.TypeFilter, request.ModelProperties);

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    buffer = stream.ToArray();
                }

                return Task.FromResult(new FileContentResult(buffer, "application/octet-stream")
                {
                    FileDownloadName = "exported.xlsx"
                });
            }
        }
    }
}
