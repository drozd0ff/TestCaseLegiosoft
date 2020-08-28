using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Controllers
{
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

        [HttpPost]
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

        [HttpPatch]
        public ActionResult UpdateSomeData()
        {
            IEnumerable<TransactionModel> someData = new List<TransactionModel>
            {
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
                }
            };

            DataTable table = new DataTable();
            table.Columns.Add(nameof(TransactionModel.TransactionId), typeof(int));
            table.Columns.Add(nameof(TransactionModel.TransactionStatus), typeof(string));
            table.Columns.Add(nameof(TransactionModel.TransactionType), typeof(string));
            table.Columns.Add(nameof(TransactionModel.ClientName), typeof(string));
            table.Columns.Add(nameof(TransactionModel.Amount), typeof(decimal));

            someData.ToList().ForEach(x => table.Rows
                .Add(x.TransactionId, x.TransactionStatus, x.TransactionType, x.ClientName, x.Amount));
            // At this moment datatable is filled with our mock data

            //foreach (DataRow tableRow in table.Rows)
            //{
            //    foreach (var item in tableRow.ItemArray)
            //    {
            //        _logger.LogInformation(item.ToString());
            //    }
            //}

            string createTmpTable = "CREATE TABLE #DataToMerge (TransactionId int, TransactionStatus nvarchar(MAX), " +
                              "TransactionType nvarchar(MAX), ClientName nvarchar(MAX), Amount decimal(13, 2))";
            
            string connectionString = Configuration.GetConnectionString("TestDatabase");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand(createTmpTable, connection);
                cmd.ExecuteNonQuery();
                
                //BulkCopy the data in the DataTable to the temp table
                using (SqlBulkCopy bulk = new SqlBulkCopy(connection))
                {
                    bulk.DestinationTableName = "#DataToMerge";
                    bulk.WriteToServer(table);
                }

                //Now use the merge command to upsert from the temp table to the production table
                string mergeSql = "SET IDENTITY_INSERT dbo.TransactionModels ON;" +
                                  "MERGE INTO TransactionModels AS Target " +
                                  "USING #DataToMerge AS Source " +
                                  "ON " +
                                  "Target.TransactionId=Source.TransactionId " +
                                  "WHEN MATCHED THEN " +
                                  "UPDATE SET Target.TransactionStatus=Source.TransactionStatus " +
                                  "WHEN NOT MATCHED THEN " +
                                  "INSERT (TransactionId, TransactionStatus, TransactionType, ClientName, Amount) " +
                                  "VALUES (Source.TransactionId, Source.TransactionStatus, " +
                                  "Source.TransactionType, Source.ClientName, Source.Amount);" +
                                  "SET IDENTITY_INSERT dbo.TransactionModels OFF;";

                cmd.CommandText = mergeSql;
                cmd.ExecuteNonQuery();

                //Clean up the temp table
                cmd.CommandText = "DROP TABLE #DataToMerge";
                cmd.ExecuteNonQuery();
            }
            
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetSomeData()
        {
            var result = await _context.TransactionModels.ToListAsync();

            return result;
        }

        [HttpDelete]
        public ActionResult DeleteSomeData()
        {
            _context.TransactionModels.RemoveRange(_context.TransactionModels.ToList());

            _context.SaveChanges();

            return Ok();
        }
    }
}
