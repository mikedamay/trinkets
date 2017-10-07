using System.Collections.Generic;

namespace com.TheDisappointedProgrammer.Drive
{
    internal interface IPivoter
    {
        (List<int> months, decimal[,] totals) Pivot(IEnumerable<AccountTotals> accountTotals);
    }
}