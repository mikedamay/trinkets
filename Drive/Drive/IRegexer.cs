using System.Collections.Generic;

namespace com.TheDisappointedProgrammer.Drive
{
    internal interface IRegexer
    {
        IList<string> SplitCsvLine(string csvLine);
    }
}