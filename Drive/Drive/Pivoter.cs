using System.CodeDom;
using System.Collections.Generic;
using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    internal class Pivoter : IPivoter
    {
        [BeanReference] private readonly ColumnMap columnMap = null;
        public (List<int> months, decimal[,] totals) Pivot(IEnumerable<AccountTotals> accountTotals)
        {
            List<int> months = new List<int>();
            decimal[,] tots = new decimal[columnMap.NumExpenseColumns,12];
            List<List<decimal>> outTotals = new List<List<decimal>>();
            int jj = 0;
            foreach (var at in accountTotals)
            {
                months.Add(at.AccountingMonth);
                int ii = 0;
                foreach (var tot in at.Summary)
                {
                    tots[ii, jj] = tot;
                    ii++;
                }
                jj++;
            }
            return (months, tots);
        }
    }
}