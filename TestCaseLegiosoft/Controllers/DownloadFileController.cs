using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace TestCaseLegiosoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadFileController : ControllerBase
    {
        public DownloadFileController()
        {
            
        }

        //[ProducesResponseType()]
        //public async Task<IActionResult> GetFile()
        //{
        //    return File(await System.IO.File.ReadAllBytesAsync())
        //}
    }
}
