using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    public class SheetTransformer : ISheetTransformer
    {
        [BeanReference] private IRegexer regexer;
        [BeanReference] private ColumnMap columnMap;

        private class Summary
        {
            private ColumnMap columnMap;
            public Summary(ColumnMap columnMap)
            {
                this.columnMap = columnMap;
                Totals = new decimal[columnMap.NumExpenseColumns];
            }

            public Summary AddExpenseLine(IList<object> csvFields)
            {
                for (int ii = columnMap.FirstExpenseColumn;
                    ii < columnMap.FirstExpenseColumn + columnMap.NumExpenseColumns;
                    ii++)
                {
                    Totals[ii - columnMap.FirstExpenseColumn] 
                      += ((decimal?)csvFields[ii]).HasValue 
                      ? (decimal)csvFields[ii] 
                      : decimal.Zero;
            }
                return this;
            }
            public decimal[] Totals { get; }
        }

        private class AccountingMonthComparer : IEqualityComparer<DateTime>
        {
            private readonly int accountingYear;
            public AccountingMonthComparer(int accountingYear)
            {
                this.accountingYear = accountingYear;
            }

            public bool Equals(DateTime x, DateTime y)
            {
                return MonthAndYearToMonth(x, accountingYear) == MonthAndYearToMonth(y, accountingYear);
            }

            public int GetHashCode(DateTime obj)
            {
                return MonthAndYearToMonth(obj, accountingYear);
            }
            /// <summary>
            /// barclay card bills go into the month following the main month to which relate
            /// </summary>
            /// <param name="dt">transaction date as recorded on Barclaycard bill</param>
            /// <returns>mostly the same as the transacton month except for the first few days
            ///   of January</returns>
            public static int MonthAndYearToMonth(DateTime dt, int accountingYear)
            {
                return dt.Year == accountingYear ? dt.Month : 12;
            }
        }
        public IEnumerable<AccountTotals> Transform(Stream sheetStream, int accountingYear)
        {
            int ctr = 1;
            try
            {
                var result = StreamToIEnum(sheetStream)
                  .Where(l => IsExpenseLine(l, ctr++)).Select(l => ParseFields(l))
                  .OrderBy(l => (DateTime)l[0])
                  .GroupBy(l => AccountingMonthComparer.MonthAndYearToMonth((DateTime)l[0], accountingYear)
                    ).Select(  g =>
                       {
                            var results = g.Aggregate(new Summary(columnMap)
                                , (acc, l) => acc.AddExpenseLine(l), acc => acc);
                            return new AccountTotals{ AccountingMonth = g.Key, Summary = results.Totals };
                        }
                  ).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failure reading sheet at line {ctr}",ex);
            }
        }

        private IList<object> ParseFields(IList<string> csvFields)
        {
            int ctr = 1;
            try
            {
                return csvFields.Select(cf => new {field = cf, index = ctr++})
                    .Join(columnMap.Columns, f => f.index, c => c.Index, (f, c) => c.Parser(f.field))
                    .Select(x => (object) x).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception($"Parser failed in ParseFields() field number {ctr} value {csvFields[ctr]}");
            }
        }

        private static bool IsExpenseLine(IList<string> fields, int dummy)
        {
            string field = fields[0];
            return !string.IsNullOrWhiteSpace(field)
              && field != "SUMMARY"
              && field != "HEADINGS";
        }

        private IEnumerable<IList<string>> StreamToIEnum(Stream sheetStream)
        {
            var nonEmptyLines = Convert(sheetStream).Where(l => l[0] != string.Empty);
            var dateLines = nonEmptyLines.Where(l => l[0] != "HEADINGS" && l[0] != "SUMMARY");

            foreach (var lineParts in Convert(sheetStream))
            { 
                yield return lineParts;
            }
        }


        private IEnumerable<IList<string>> Convert(Stream sheetStream)
        {
            StreamReader reader = new StreamReader(sheetStream);
            
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                IList<string> lineParts = regexer.SplitCsvLine(line);
                yield return lineParts;
            }
        }
    }
}