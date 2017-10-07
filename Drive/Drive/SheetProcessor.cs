using System.Collections.Generic;
using System.IO;
using System.Linq;
using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    public class SheetProcessor
    {
        [BeanReference] private ILogger logger = null;
        [BeanReference] private readonly IGoogleSheetLoader googleSheetLoader = null;
        [BeanReference] private readonly ISheetTransformer transformer = null;
        [BeanReference(Factory=typeof(AccountingYearFactory))] private int accountingYear;
        [BeanReference] private readonly IPivoter pivoter = null;
        [BeanReference] private readonly IReportBuilder reportBuilder = null;
        [BeanReference] private readonly ColumnMap columnMap = null;

        public void Process()
        {
            byte[] sheetBytes = googleSheetLoader.LoadSheet("MDAM-EXES-2017");
            var msin = new MemoryStream(sheetBytes);
            var accountGroups = transformer.Transform(msin, accountingYear);
            (var months, var totals) = pivoter.Pivot(accountGroups);
            var report = reportBuilder.BuildReport(months, totals, columnMap);
            var msout = new MemoryStream();
            var sw = new StreamWriter(msout);
            foreach (var line in report)
            {
                sw.WriteLine(line);
            }
            sw.Close();
            googleSheetLoader.SaveSheet("Finance", "ExesSummary2017", new MemoryStream(msout.GetBuffer()));
        }
    }

    [Bean]
    internal class AccountingYearFactory : IFactory
    {
        public object Execute(BeanFactoryArgs args)
        {
            return 2017;
        }
    }

    public static class Extensions
    {
        public static void ToNull(this IEnumerable<bool> source)
        {
            IEnumerator<bool> ienum = source.GetEnumerator();
            while (ienum.MoveNext()) ;
        }
    }
}
