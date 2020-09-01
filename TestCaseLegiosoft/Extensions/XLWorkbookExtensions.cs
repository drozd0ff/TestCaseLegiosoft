using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClosedXML.Excel;
using TestCaseLegiosoft.Models;
using TestCaseLegiosoft.Models.Enums;
using TestCaseLegiosoft.Persistence;

namespace TestCaseLegiosoft.Extensions
{
    public static class XLWorkbookExtensions
    {
        private static void FillWorksheetWithQuery(this IXLWorksheet ws, IQueryable query)
        {
            ws.Cell(1, 1).Value = nameof(TransactionModel.TransactionId);
            ws.Cell(1, 2).Value = nameof(TransactionModel.TransactionStatus);
            ws.Cell(1, 3).Value = nameof(TransactionModel.TransactionType);
            ws.Cell(1, 4).Value = nameof(TransactionModel.ClientName);
            ws.Cell(1, 5).Value = nameof(TransactionModel.Amount);

            ws.Cell(2, 1).Value = query;

            ws.Columns().AdjustToContents();
        }

        private static void FillWorksheetWithQuery(this IXLWorksheet ws, IQueryable query, 
            Dictionary<PropertyInfo, bool> properties)
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < properties.Count; i++)
            {
                ws.Cell(1, i + 1).Value = properties.ElementAt(i).Key.Name;
                if (!properties.ElementAt(i).Value)
                {
                    indexes.Add(i + 1);
                }
            }

            ws.Cell(2, 1).Value = query;

            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                ws.Column(indexes[i]).Delete();
            }

            ws.Columns().AdjustToContents();
        }

        public static void InsertData(this XLWorkbook workbook, DataContext dataContext)
        {
            var ws = workbook.Worksheets.Add("Export sheet");

            var transactions = dataContext.TransactionModels
                .Select(x =>
                    new {x.TransactionId, x.TransactionStatus, x.TransactionType, x.ClientName, x.Amount});

            ws.FillWorksheetWithQuery(transactions);
        }

        public static void InsertData(this XLWorkbook workbook, DataContext dataContext,
            TransactionStatus statusFilter, Dictionary<PropertyInfo, bool> properties)
        {
            var ws = workbook.Worksheets.Add("Export sheet");

            var transactions = dataContext.TransactionModels
                .Where(x => x.TransactionStatus == statusFilter)
                .Select(x =>
                    new { x.TransactionId, x.TransactionStatus, x.TransactionType, x.ClientName, x.Amount });

            ws.FillWorksheetWithQuery(transactions, properties);
        }

        public static void InsertData(this XLWorkbook workbook, DataContext dataContext,
            TransactionType typeFilter, Dictionary<PropertyInfo, bool> properties)
        {
            var ws = workbook.Worksheets.Add("Export sheet");

            var transactions = dataContext.TransactionModels
                .Where(x => x.TransactionType == typeFilter)
                .Select(x =>
                    new { x.TransactionId, x.TransactionStatus, x.TransactionType, x.ClientName, x.Amount });

            ws.FillWorksheetWithQuery(transactions, properties);
        }
    }
}
