using System;
using System.Collections.Generic;
using System.IO;

namespace com.TheDisappointedProgrammer.Drive
{
    internal interface ISheetTransformer
    {
        IEnumerable<AccountTotals> Transform(Stream sheetStream);
    }
}