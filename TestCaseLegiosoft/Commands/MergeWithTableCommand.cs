using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;

namespace TestCaseLegiosoft.Commands
{
    public class MergeWithTableCommand : IRequest<string>
    {
        public IFormFile File { get; }

        public MergeWithTableCommand(IFormFile file)
        {
            File = file;
        }
    }

    public class MergeWithTableHandler : IRequestHandler<MergeWithTableCommand, string>
    {
        public IConfiguration Configuration { get; }

        public MergeWithTableHandler(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Task<string> Handle(MergeWithTableCommand request, CancellationToken cancellationToken)
        {
            // Since we are not allowed to use automapper
            // (As it seems to me all third party CSV parsers are automappers at some point)
            // we will try to parse CSV manually
            // Also we assume that input CSV file really have commas as separators,
            // fields don't have extra whitespaces and aren't enclosed in quotes
            try
            {
                DataTable table = FillDataTableWithStream(request.File.OpenReadStream());

                MergeTable(table, Configuration.GetConnectionString("TestDatabase"));

                return Task.FromResult("Successfully merged!");
            }
            catch (Exception ex)
            {
                return Task.FromResult(ex.Message);
            }
        }

        private static DataTable FillDataTableWithStream(Stream stream)
        {
            DataTable table = new DataTable();

            table.Columns.Add(nameof(TransactionModel.TransactionId), typeof(int));
            table.Columns.Add(nameof(TransactionModel.TransactionStatus), typeof(string));
            table.Columns.Add(nameof(TransactionModel.TransactionType), typeof(string));
            table.Columns.Add(nameof(TransactionModel.ClientName), typeof(string));
            table.Columns.Add(nameof(TransactionModel.Amount), typeof(decimal));

            using (StreamReader streamReader = new StreamReader(stream))
            {
                // Assume that first line is column names
                streamReader.ReadLine();

                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    if (!String.IsNullOrWhiteSpace(line))
                    {
                        string[] values = line.Split(',');
                        if (values.Length >= 5
                            | Decimal.TryParse(values[4], NumberStyles.Currency,
                                new CultureInfo("en-US"), out decimal v))
                        {
                            table.Rows.Add(
                                int.Parse(values[0]),
                                Enum.Parse<TransactionStatus>(values[1]),
                                Enum.Parse<TransactionType>(values[2]),
                                values[3],
                                v
                            );
                        }
                    }
                }
            }

            return table;
        }

        private static void MergeTable(DataTable table, string connectionString)
        {
            string createTmpTable = "CREATE TABLE #DataToMerge (TransactionId int, TransactionStatus nvarchar(MAX), " +
                              "TransactionType nvarchar(MAX), ClientName nvarchar(MAX), Amount decimal(13, 2))";

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
                                  "MERGE INTO TransactionModels WITH (HOLDLOCK) AS Target " +
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
        }
    }
}
