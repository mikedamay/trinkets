using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    public class ReportBuilder : IReportBuilder
    {
        public IEnumerable<string> BuildReport(IList<int> months, decimal[,] totals, ColumnMap columnMap)
        {
            string MonthName(int m) => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m);
            StringBuilder sb = new StringBuilder();
            sb.Append("abc,");
            sb.Append(MonthName(months[0]));
            foreach (int month in months.Skip(1))
            {
                sb.Append(",");
                sb.Append(MonthName(month));
            }
            yield return sb.ToString();
            for (int ii = 0; ii < totals.GetLength(0); ii++)
            {
                sb.Clear();
                sb.Append(columnMap.Columns[ii + columnMap.FirstExpenseColumn].Heading);
                for (int jj = 0; jj < months.Count; jj++)
                {
                    sb.Append(",");
                    sb.Append(totals[ii, jj]);
                }
                yield return sb.ToString();
            }
        }
    }
}