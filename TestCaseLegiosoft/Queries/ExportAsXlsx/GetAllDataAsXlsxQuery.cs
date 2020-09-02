using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TestCaseLegiosoft.Extensions;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Queries
{
    public class GetAllDataAsXlsxQuery : IRequest<FileContentResult>
    { }

    public class GetAllDataAsXlsxHandler : IRequestHandler<GetAllDataAsXlsxQuery, FileContentResult>
    {
        private readonly DataContext _dataContext;

        public GetAllDataAsXlsxHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<FileContentResult> Handle(GetAllDataAsXlsxQuery request, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[16 * 1024];
            using (var workbook = new XLWorkbook())
            {
                workbook.InsertData(_dataContext);

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
