using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TestCaseLegiosoft.Extensions;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Queries.ExportAsXlsx.GetDataByTypeAsXlsx
{
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
