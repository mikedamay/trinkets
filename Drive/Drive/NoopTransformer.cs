using System.Collections.Generic;
using System.IO;
using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean(Profile="NoopTransformer")]
    public class NoopTransformer : ISheetTransformer
    {
        public IEnumerable<AccountTotals> Transform(Stream sheetStream, int accountingYear)
        {
            StreamReader reader = new StreamReader(sheetStream);
            string line;
            while ( (line = reader.ReadLine()) != null )
            {
                yield return new AccountTotals();
            }
        }
    }
}