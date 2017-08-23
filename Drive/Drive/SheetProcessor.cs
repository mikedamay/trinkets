using System;
using System.Collections.Generic;
using System.IO;
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
        [BeanReference] private readonly IGoogleSheetLoader googleSheetLoader = null;
        [BeanReference] private readonly ISheetTransformer transformer = null;

        public void Process()
        {
            byte[] sheetBytes = googleSheetLoader.LoadSheet("MDAM-EXES-2017");

            System.IO.StreamReader sr = new StreamReader(new MemoryStream(sheetBytes));
            var msin = new MemoryStream(sheetBytes);
            var msout = new MemoryStream();
            StreamWriter sw = new StreamWriter(msout);
            foreach (var line in transformer.Transform(msin))
            {
                sw.WriteLine(line);
            }
            //string str = sr.ReadToEnd();
            //var ms = new MemoryStream(Encoding.UTF8.GetBytes(str));
            googleSheetLoader.SaveSheet("Finance", "MyLateatStuff", msout);
            Console.WriteLine(sheetBytes.Length);
            Console.Read();

        }
    }
}
