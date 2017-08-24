using System.Collections.Generic;

namespace com.TheDisappointedProgrammer.Drive
{
    internal interface IReportBuilder
    {
        IEnumerable<string> BuildReport(IList<int> months, decimal[,] totals, ColumnMap columnMap);
    }
}