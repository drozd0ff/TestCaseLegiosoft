using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MockDbController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        private readonly DataContext _context;
        private readonly ILogger<MockDbController> _logger;

        public MockDbController(DataContext context, ILogger<MockDbController> logger, IConfiguration configuration)
        {
            Configuration = configuration;
            _context = context;
            _logger = logger;
        }

        [HttpPost("AddSomeDataToDb")]
        public async Task<ActionResult> AddSomeDataToDb()
        {
            IEnumerable<TransactionModel> someData = new List<TransactionModel>
            {
                new TransactionModel
                {
                    TransactionId = 1, TransactionStatus = TransactionStatus.Completed,
                    TransactionType = TransactionType.Withdrawal, ClientName = "Dale Cotton", Amount = 28.43m
                },
                new TransactionModel
                {
                    TransactionId = 2, TransactionStatus = TransactionStatus.Pending,
                    TransactionType = TransactionType.Refill, ClientName = "Paul Carter", Amount = 45.16m
                },
                new TransactionModel
                {
                    TransactionId = 3, TransactionStatus = TransactionStatus.Cancelled,
                    TransactionType = TransactionType.Refill, ClientName = "Caldwell Reid", Amount = 63.00m
                },
                new TransactionModel
                {
                    TransactionId = 4, TransactionStatus = TransactionStatus.Cancelled,
                    TransactionType = TransactionType.Refill, ClientName = "Quentin Bonner", Amount = 64.52m
                },
                new TransactionModel
                {
                    TransactionId = 5, TransactionStatus = TransactionStatus.Cancelled,
                    TransactionType = TransactionType.Withdrawal, ClientName = "Colt Joyce", Amount = 70.67m
                }
            };

            await _context.TransactionModels.AddRangeAsync(someData);

            _context.Database.OpenConnection();
            try
            {
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.TransactionModels ON");
                _context.SaveChanges();
                _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.TransactionModels OFF");
            }
            finally
            {
                _context.Database.CloseConnection();
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("GetAllData")]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetSomeData()
        {
            var result = await _context.TransactionModels.ToListAsync();

            return result;
        }

        [HttpDelete("RemoveAllData")]
        public ActionResult DeleteSomeData()
        {
            _context.TransactionModels.RemoveRange(_context.TransactionModels.ToList());

            _context.SaveChanges();

            return Ok();
        }
    }
}
