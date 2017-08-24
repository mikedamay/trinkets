using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using com.TheDisappointedProgrammer.IOCC;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean]
    public class SheetProcessor
    {
        [BeanReference] private ILogger logger = null;
        [BeanReference] private readonly IGoogleSheetLoader googleSheetLoader = null;
        [BeanReference] private readonly ISheetTransformer transformer = null;

        public void Process()
        {
            byte[] sheetBytes = googleSheetLoader.LoadSheet("MDAM-EXES-2017");

            var msin = new MemoryStream(sheetBytes);
            foreach (var line in transformer.Transform(msin))
            {
                logger.Log(line.AccountingMonth);
                line.Summary.Select(s => logger.Log($" {s}")).ToList();
                logger.LogLine("");
            }
            logger.LogLine(sheetBytes.Length);

        }
    }
}
