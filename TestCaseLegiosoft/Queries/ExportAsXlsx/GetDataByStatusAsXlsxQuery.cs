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

    public class GetDataByStatusAsXlsxHandler : IRequestHandler<GetDataByStatusAsXlsxQuery, FileContentResult>
    {
        private readonly DataContext _dataContext;

        public GetDataByStatusAsXlsxHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<FileContentResult> Handle(GetDataByStatusAsXlsxQuery request, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[16 * 1024];
            using (var workbook = new XLWorkbook())
            {
                workbook.InsertData(_dataContext, request.StatusFilter, request.ModelProperties);

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
