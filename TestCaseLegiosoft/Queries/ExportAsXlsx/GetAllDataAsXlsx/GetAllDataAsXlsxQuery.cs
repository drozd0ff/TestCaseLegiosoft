using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TestCaseLegiosoft.Queries.ExportAsXlsx.GetAllDataAsXlsx
{
    public class GetAllDataAsXlsxQuery : IRequest<FileContentResult>
    { }
}