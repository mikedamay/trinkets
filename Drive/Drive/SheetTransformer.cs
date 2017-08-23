using System;
using System.Collections.Generic;
using System.IO;

using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    public class SheetTransformer : ISheetTransformer
    {
        [BeanReference] private IRegexer regexer;
        public IEnumerable<string> Transform(Stream sheetStream)
        {
            StreamReader reader = new StreamReader(sheetStream);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string lineOut = TransformLine(line);
                yield return lineOut;
            }
        }

        private string TransformLine(string line)
        {
            IList<string> lineParts = regexer.SplitCsvLine(line);
            return String.Join(",", lineParts);
        }
    }
}