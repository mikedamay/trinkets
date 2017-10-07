using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using com.TheDisappointedProgrammer.IOCC;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    public class Regexer : IRegexer
    {
        public IList<string> SplitCsvLine(string csvLine)
        {
            // https://stackoverflow.com/questions/3268622/regex-to-split-line-csv-file
            List<string> lineParts = new List<string>();
            try
            {
                Regex pattern = new Regex(@"
                # Parse CVS line. Capture next value in named group: 'val'
                \s*                      # Ignore leading whitespace.
                (?:                      # Group of value alternatives.
                  ""                     # Either a double quoted string,
                  (?<val>                # Capture contents between quotes.
                    [^""]*(""""[^""]*)*  # Zero or more non-quotes, allowing 
                  )                      # doubled "" quotes within string.
                  ""\s*                  # Ignore whitespace following quote.
                |  (?<val>[^,]*)         # Or... zero or more non-commas.
                )                        # End value alternatives group.
                (?:,|$)                  # Match end is comma or EOS",
                     RegexOptions.IgnorePatternWhitespace);
                Match matchResult = pattern.Match(csvLine);
                while (matchResult.Success)
                {
                    lineParts.Add(matchResult.Groups["val"].Value);
                    matchResult = matchResult.NextMatch();
                }
            }
            catch (ArgumentException ex)
            {
                throw;
                // Syntax error in the regular expression
            }
            return lineParts;
        }
    }
}