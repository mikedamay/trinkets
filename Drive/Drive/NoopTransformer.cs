using System.Collections.Generic;
using System.IO;
using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean(Profile="NoopTransformer")]
    public class NoopTransformer : ISheetTransformer
    {
        public IEnumerable<string> Transform(Stream sheetStream)
        {
            StreamReader reader = new StreamReader(sheetStream);
            string line;
            while ( (line = reader.ReadLine()) != null )
            {
                yield return line;
            }
        }
    }
}