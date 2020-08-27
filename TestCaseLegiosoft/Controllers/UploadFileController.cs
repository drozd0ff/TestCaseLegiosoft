using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestCaseLegiosoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private static IWebHostEnvironment _environment;

        public UploadFileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public class UploadFileAPI
        {
            public IFormFile Files { get; set; }
        }

        [HttpPost]
        public async Task<string> Post(UploadFileAPI objFile)
        {
            try
            {
                if (objFile.Files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Upload\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                    }

                    using (FileStream fileStream = System.IO.File
                        .Create(_environment.WebRootPath + "\\Upload\\" + objFile.Files.FileName))
                    {
                        objFile.Files.CopyTo(fileStream);
                        fileStream.Flush();
                        return "\\Upload\\" + objFile.Files.FileName;
                    }
                }

                return "Failed";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
