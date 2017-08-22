using System;
using System.Collections.Generic;
using System.IO;
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
        //[BeanReference(Factory=typeof(DriveServiceFactory))]
        //private DriveService driveService;

        [BeanReference(Name="bylist")] private IGoogleSheetLoader googleSheetLoader;

        //[BeanReference] private IFileLister fileLister;

        public void Process()
        {
            byte[] sheetBytes = googleSheetLoader.LoadSheet("MDAM-EXES-2017");

            //List<(string fileName, string fileId)> fileSpecs = fileLister.ListFiles();

            //Console.WriteLine("Files:");
            //if (fileSpecs != null && fileSpecs.Count > 0)
            //{
            //    foreach (var file in fileSpecs)
            //    {
            //        Console.WriteLine("{0} ({1})", file.fileName, file.fileId);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No files found.");
            //}
            Console.WriteLine(sheetBytes.Length);
            Console.Read();

        }
    }
}
