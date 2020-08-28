using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MockDbController : ControllerBase
    {
        private readonly DataContext _context;

        public MockDbController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> AddSomeDataToDb()
        {
            IEnumerable<TransactionModel> someData = new List<TransactionModel>
            {
                new TransactionModel
                {
                    TransactionId = 1, TransactionStatus = TransactionStatus.Pending,
                    TransactionType = TransactionType.Withdrawal, ClientName = "Dale Cotton", Amount = 28.43m
                },
                new TransactionModel
                {
                    TransactionId = 2, TransactionStatus = TransactionStatus.Completed,
                    TransactionType = TransactionType.Refill, ClientName = "Paul Carter", Amount = 45.16m
                },
                new TransactionModel
                {
                    TransactionId = 3, TransactionStatus = TransactionStatus.Cancelled,
                    TransactionType = TransactionType.Refill, ClientName = "Caldwell Reid", Amount = 63.00m
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetSomeData()
        {
            var result = await _context.TransactionModels.ToListAsync();

            return result;
        }
    }
}
