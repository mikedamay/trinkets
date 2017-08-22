using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using com.TheDisappointedProgrammer.IOCC;
using Google.Apis.Drive.v3;

namespace com.TheDisappointedProgrammer.Drive
{
    [Bean(Name="bylist")]
    public class CsapiGoogleSheetLoaderByList : IGoogleSheetLoader
    {
        [BeanReference] private IFileLister fileLister;
        [BeanReference(Factory = typeof(DriveServiceFactory))] private DriveService driveService;
        public byte[] LoadSheet(string sheetName)
        {
            IList<(string fileName, string fileId)> fileSpecs
              = fileLister.ListFiles();
            MemoryStream stream = new MemoryStream();
            string matchingFileId = fileSpecs.Where(
              f => f.fileName == sheetName).Select(f => f.fileId)
              .FirstOrDefault();
            if (matchingFileId == default(string))
            {
                throw new Exception($"Failed to find a google sheet for {sheetName}");
            }
            var request = driveService.Files.Export(matchingFileId, "text/csv");
            if (request == null)
            {
                throw new Exception($"Failed to create a request for {sheetName} {matchingFileId}");
            }
            request.Download(stream);
            return stream.GetBuffer();
        }
    }
}