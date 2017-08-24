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

        public void Process()
        {
            byte[] sheetBytes = googleSheetLoader.LoadSheet("MDAM-EXES-2017");

            var msin = new MemoryStream(sheetBytes);
            foreach (var line in transformer.Transform(msin, accountingYear))
            {
                logger.Log(line.AccountingMonth);
                line.Summary.Select(s => logger.Log($" {s}")).ToNull();
                logger.LogLine("");
            }
            logger.LogLine(sheetBytes.Length);

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
