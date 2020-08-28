using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFileController : ControllerBase
    {
        private readonly ILogger<UploadFileController> _logger;
        private readonly DataContext _context;

        public UploadFileController(ILogger<UploadFileController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Post(IFormFile file)
        {
            // Since we are not allowed to use automapper
            // (I believe all third party CSV parsers are automappers at some point)
            // we will try to parse CSV manually
            // Also we assume that input CSV file really have commas as separators,
            // fields don't have extra whitespaces and aren't enclosed in quotes
            try
            {
                List<string> list0 = new List<string>();
                List<string> list1 = new List<string>();
                List<string> list2 = new List<string>();
                List<string> list3 = new List<string>();
                List<string> list4 = new List<string>();

                List<TransactionModel> updateList = new List<TransactionModel>();

                using (StreamReader streamReader = new StreamReader(file.OpenReadStream()))
                {
                    // Assume that first line is column names
                    streamReader.ReadLine();

                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        if (!String.IsNullOrWhiteSpace(line))
                        {
                            string[] values = line.Split(',');
                            _context.TransactionModels.UpdateRange();
                            if (values.Length >= 5)
                            {
                                //updateList.Add(new TransactionModel
                                //{
                                //    TransactionId = int.TryParse(values[0], out int v),
                                //    TransactionStatus = values[1],
                                //    TransactionType = values[2],
                                //    ClientName = values[3],
                                //    Amount = values[4]
                                //});

                                //list0.Add(values[0]);
                                //list1.Add(values[1]);
                                //list2.Add(values[2]);
                                //list3.Add(values[3]);
                                //list4.Add(values[4]);
                            }
                        }
                    }
                }

                return list1[0];
            }
            catch (Exception ex)
            {
                Response.StatusCode = 400;
                return Content(ex.Message);
            }
        }
    }
}
